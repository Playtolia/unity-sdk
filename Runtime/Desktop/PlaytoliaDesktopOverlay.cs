using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

namespace Playtolia.Desktop
{
    /// <summary>
    /// Desktop (Windows) host for the Playtolia SDK: starts the native SDK, pumps its main loop every
    /// frame, shows the SDK UI (rendered natively with Compose into a texture) as a full-screen
    /// overlay, and forwards legacy Input Manager events to it. Started by PlaytoliaGameObject.
    /// </summary>
    public sealed class PlaytoliaDesktopOverlay : MonoBehaviour
    {
        private const string LogPrefix = "[Playtolia] ";
        private const int SortingOrder = 32000;

        // Pointer event types / buttons / modifiers (CONTRACT.md).
        private const int EvMove = 0, EvPress = 1, EvRelease = 2, EvScroll = 3, EvEnter = 4, EvExit = 5;
        private const int ModShift = 1, ModCtrl = 2, ModAlt = 4, ModMeta = 8;

        // playtolia_status(): 0 not started, 1 starting, 2 ready, 3 failed.
        private const int StatusFailed = 3;

        private static PlaytoliaDesktopOverlay s_instance;
        private static bool s_bootstrapped;
        private static bool s_nativeAlive;

        /// <summary>True when the overlay is visible and draws something opaque under the cursor.
        /// Game code can check this to ignore clicks meant for the overlay.</summary>
        public static bool PointerOverUi { get; private set; }

        /// <summary>True while the SDK UI has the keyboard (a sheet or dialog is open). Game code
        /// can check this to ignore keys meant for the overlay, Escape included.</summary>
        public static bool KeyboardCaptured { get; private set; }

        /// <summary>The running overlay, or null when the native library is unavailable.</summary>
        public static PlaytoliaDesktopOverlay Instance => s_instance;

        public bool Visible => _visible;

        private Canvas _canvas;
        private RawImage _image;
        private Material _material;
        private Texture2D _texture;
        private byte[] _pixels;
        private GCHandle _pixelsHandle;
        private IntPtr _pixelsPtr = IntPtr.Zero;
        private int _width, _height;
        private float _density = 1f;
        private readonly Stopwatch _clock = new Stopwatch();
        private bool _forceNextFrame = true;
        private bool _visible = true;
        private bool _renderErrorLogged;

        // Input state
        private Vector2 _lastPointer = new Vector2(float.NaN, float.NaN);
        private bool _pointerInside;
        private int _heldButtons;

        private static readonly KeyMapping[] s_keys = BuildKeyMap();

        private struct KeyMapping
        {
            public KeyCode Unity;
            public int Vk;

            public KeyMapping(KeyCode unity, int vk)
            {
                Unity = unity;
                Vk = vk;
            }
        }

        // Survives "Enter Play Mode" with domain reload disabled.
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics()
        {
            s_instance = null;
            s_bootstrapped = false;
            s_nativeAlive = false;
            PointerOverUi = false;
            KeyboardCaptured = false;
        }

        /// <summary>Platforms the desktop library ships for.</summary>
        public static bool IsSupportedPlatform => Application.platform == RuntimePlatform.WindowsPlayer;

        /// <summary>
        /// Starts the SDK with the game's configuration (written next to the player by the build;
        /// see PlaytoliaDesktopBuild) and creates the overlay. Safe to call more than once.
        /// </summary>
        public static void Initialize()
        {
            if (s_bootstrapped || !IsSupportedPlatform) return;
            s_bootstrapped = true;

            var configPath = System.IO.Path.Combine(Application.dataPath, "Playtolia", "playtolia_conf.json");
            if (!System.IO.File.Exists(configPath))
            {
                Debug.LogError(LogPrefix + "configuration not found at " + configPath + "; the SDK was not started.");
                return;
            }

            try
            {
                // Blocks until the SDK is ready (like the iOS bridge), so game code calling the SDK
                // from Start() sees it initialized. A slow network only delays readiness.
                if (!PlaytoliaDesktopNative.Attach(System.IO.File.ReadAllText(configPath)))
                {
                    if (PlaytoliaDesktopNative.playtolia_status() == StatusFailed)
                    {
                        Debug.LogError(LogPrefix + "SDK failed to start: " + (PlaytoliaDesktopNative.LastError() ?? "unknown error"));
                        return;
                    }
                    Debug.LogWarning(LogPrefix + "SDK is still starting; it will finish in the background.");
                }
            }
            catch (DllNotFoundException e)
            {
                Debug.LogError(LogPrefix + "native library 'playtolia' not found; the SDK was not started. (" + e.Message + ")");
                return;
            }

            var go = new GameObject("PlaytoliaDesktopOverlay");
            DontDestroyOnLoad(go);
            go.AddComponent<PlaytoliaDesktopOverlay>();
        }

        private void Awake()
        {
            if (s_instance != null && s_instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _width = Mathf.Max(1, Screen.width);
            _height = Mathf.Max(1, Screen.height);
            _density = ComputeDensity();

            // First native call: the library may be missing on this platform/build.
            try
            {
                PlaytoliaDesktopNative.InstallLogCallback();
                if (PlaytoliaDesktopNative.ptui_create(_width, _height, _density) != 1)
                {
                    Debug.LogError(LogPrefix + "ptui_create failed: " + (PlaytoliaDesktopNative.LastError() ?? "unknown error"));
                    PlaytoliaDesktopNative.RemoveLogCallback();
                    enabled = false;
                    return;
                }
            }
            catch (DllNotFoundException e)
            {
                Debug.LogWarning(LogPrefix + "native library 'playtolia' not found, overlay disabled. (" + e.Message + ")");
                enabled = false;
                return;
            }
            catch (EntryPointNotFoundException e)
            {
                Debug.LogWarning(LogPrefix + "native library 'playtolia' is missing an entry point " +
                                 "(stale build?), overlay disabled. (" + e.Message + ")");
                enabled = false;
                return;
            }

            s_nativeAlive = true;
            s_instance = this;

            BuildCanvas();
            AllocateSurface(_width, _height);
            _clock.Start();
            Debug.Log(LogPrefix + "overlay started at " + _width + "x" + _height + " density " + _density.ToString("0.##"));
        }

        private void BuildCanvas()
        {
            _canvas = gameObject.AddComponent<Canvas>();
            _canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            _canvas.sortingOrder = SortingOrder;
            _canvas.pixelPerfect = false;

            var scaler = gameObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
            scaler.scaleFactor = 1f;
            // No GraphicRaycaster: the overlay never steals uGUI raycasts from the game.

            var imageGo = new GameObject("PlaytoliaDesktopSurface");
            imageGo.transform.SetParent(transform, false);
            var rect = imageGo.AddComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;

            _image = imageGo.AddComponent<RawImage>();
            _image.raycastTarget = false;
            _image.color = Color.white;
            // Skia rows are top-to-bottom; Unity textures are bottom-to-top.
            _image.uvRect = new Rect(0f, 1f, 1f, -1f);

            var shader = Resources.Load<Shader>("PlaytoliaPremultipliedUI");
            if (shader == null) shader = Shader.Find("Hidden/Playtolia/PremultipliedUI");
            if (shader == null)
            {
                Debug.LogWarning(LogPrefix + "premultiplied UI shader not found, falling back to UI/Default " +
                                 "(edges and translucent areas will look too dark).");
                shader = Shader.Find("UI/Default");
            }

            if (shader != null)
            {
                _material = new Material(shader) { name = "PlaytoliaPlaytoliaDesktopOverlay" };
                _image.material = _material;
            }
        }

        private void AllocateSurface(int width, int height)
        {
            ReleasePixels();

            _pixels = new byte[width * height * 4];
            _pixelsHandle = GCHandle.Alloc(_pixels, GCHandleType.Pinned);
            _pixelsPtr = _pixelsHandle.AddrOfPinnedObject();

            if (_texture != null) Destroy(_texture);
            _texture = new Texture2D(width, height, TextureFormat.BGRA32, false, false)
            {
                name = "PlaytoliaDesktopSurface",
                filterMode = FilterMode.Point,
                wrapMode = TextureWrapMode.Clamp
            };
            if (_image != null) _image.texture = _texture;

            _width = width;
            _height = height;
            _forceNextFrame = true;
        }

        private void ReleasePixels()
        {
            if (_pixelsHandle.IsAllocated) _pixelsHandle.Free();
            _pixelsPtr = IntPtr.Zero;
            _pixels = null;
        }

        // The SDK UI is designed for phone-sized screens (bottom sheets, ~400dp tall in landscape),
        // so use the OS scale and only shrink when the window is shorter than this.
        private const float MinLayoutHeightDp = 400f;

        private static float ComputeDensity()
        {
            float dpi = Screen.dpi;
            float osDensity = dpi > 0f ? dpi / 96f : 1f;
            float fitDensity = Screen.height / MinLayoutHeightDp;
            return Mathf.Clamp(Mathf.Min(osDensity, fitDensity), 0.75f, 4f);
        }

        private void Update()
        {
            if (!s_nativeAlive) return;

            // The SDK's main thread is this one: run its queued work every frame, even with no UI.
            PlaytoliaDesktopNative.playtolia_tick();
            PlaytoliaDesktopNative.DrainMessages();

            HandleResize();

            if (!_visible)
            {
                PointerOverUi = false;
                KeyboardCaptured = false;
                return;
            }

            ForwardInput();
            RenderFrame();
        }

        /// <summary>Shows or hides the overlay. While hidden no input is forwarded and nothing is rendered.</summary>
        public void SetVisible(bool visible)
        {
            if (_visible == visible) return;
            _visible = visible;
            if (_canvas != null) _canvas.enabled = visible;
            if (visible)
            {
                _forceNextFrame = true;
                _lastPointer = new Vector2(float.NaN, float.NaN);
            }
            else
            {
                // Release whatever the UI thinks is held so it doesn't stay pressed while hidden.
                if (_heldButtons != 0 && _pointerInside)
                {
                    var p = _lastPointer;
                    for (int b = 1; b <= 3; b++)
                    {
                        int bit = ButtonBit(b);
                        if ((_heldButtons & bit) == 0) continue;
                        _heldButtons &= ~bit;
                        PlaytoliaDesktopNative.ptui_pointer(EvRelease, p.x, p.y, b, _heldButtons, 0f, 0f, CurrentModifiers());
                    }
                }
                _heldButtons = 0;
                PointerOverUi = false;
                KeyboardCaptured = false;
            }
        }

        private void HandleResize()
        {
            int w = Screen.width;
            int h = Screen.height;
            if (w <= 0 || h <= 0) return; // minimized
            float density = ComputeDensity();
            if (w == _width && h == _height && Mathf.Approximately(density, _density)) return;

            _density = density;
            PlaytoliaDesktopNative.ptui_resize(w, h, density);
            AllocateSurface(w, h);
        }

        private void RenderFrame()
        {
            if (_pixelsPtr == IntPtr.Zero) return;

            long nanos = (long)(_clock.ElapsedTicks * (1_000_000_000.0 / Stopwatch.Frequency));
            int result = PlaytoliaDesktopNative.ptui_render(nanos, _pixelsPtr, _forceNextFrame ? 1 : 0);
            if (result == 1)
            {
                _forceNextFrame = false;
                _texture.LoadRawTextureData(_pixels);
                _texture.Apply(false);
            }
            else if (result < 0)
            {
                if (!_renderErrorLogged)
                {
                    _renderErrorLogged = true;
                    Debug.LogError(LogPrefix + "ptui_render failed: " + (PlaytoliaDesktopNative.LastError() ?? "unknown error") +
                                   " (further render errors suppressed)");
                }
            }
        }

        // ---------------------------------------------------------------- input

        private void ForwardInput()
        {
            int mods = CurrentModifiers();

            Vector3 mp = Input.mousePosition;
            float x = mp.x;
            float y = Screen.height - mp.y; // top-left origin
            var pos = new Vector2(x, y);
            bool inside = x >= 0f && y >= 0f && x < Screen.width && y < Screen.height;

            if (inside && !_pointerInside)
            {
                PlaytoliaDesktopNative.ptui_pointer(EvEnter, x, y, 0, _heldButtons, 0f, 0f, mods);
                _pointerInside = true;
            }

            if (pos != _lastPointer && !(float.IsNaN(pos.x)))
            {
                if (inside || _heldButtons != 0)
                {
                    PlaytoliaDesktopNative.ptui_pointer(EvMove, x, y, 0, _heldButtons, 0f, 0f, mods);
                }
                _lastPointer = pos;
            }

            for (int unityButton = 0; unityButton < 3; unityButton++)
            {
                int button = unityButton + 1; // 0->1 primary, 1->2 secondary, 2->3 tertiary
                int bit = ButtonBit(button);
                if (Input.GetMouseButtonDown(unityButton) && inside)
                {
                    _heldButtons |= bit;
                    PlaytoliaDesktopNative.ptui_pointer(EvPress, x, y, button, _heldButtons, 0f, 0f, mods);
                }
                if (Input.GetMouseButtonUp(unityButton) && (_heldButtons & bit) != 0)
                {
                    _heldButtons &= ~bit;
                    PlaytoliaDesktopNative.ptui_pointer(EvRelease, x, y, button, _heldButtons, 0f, 0f, mods);
                }
            }

            Vector2 scroll = Input.mouseScrollDelta;
            if (inside && (scroll.x != 0f || scroll.y != 0f))
            {
                PlaytoliaDesktopNative.ptui_pointer(EvScroll, x, y, 0, _heldButtons, scroll.x, scroll.y, mods);
            }

            if (!inside && _pointerInside && _heldButtons == 0)
            {
                PlaytoliaDesktopNative.ptui_pointer(EvExit, x, y, 0, 0, 0f, 0f, mods);
                _pointerInside = false;
            }

            PointerOverUi = inside && PlaytoliaDesktopNative.ptui_hit_test(x, y) == 1;

            // Keyboard: raw keys (VK codes) first, then committed text.
            for (int i = 0; i < s_keys.Length; i++)
            {
                var k = s_keys[i];
                if (Input.GetKeyDown(k.Unity)) PlaytoliaDesktopNative.ptui_key(0, k.Vk, mods);
                if (Input.GetKeyUp(k.Unity)) PlaytoliaDesktopNative.ptui_key(1, k.Vk, mods);
            }

            string text = FilterText(Input.inputString);
            if (text.Length > 0) PlaytoliaDesktopNative.SendText(text);

            KeyboardCaptured = PlaytoliaDesktopNative.ptui_wants_keyboard() == 1;
        }

        private static int ButtonBit(int button)
        {
            // 1 -> 1, 2 -> 2, 3 -> 4
            return 1 << (button - 1);
        }

#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(int virtualKey);

        private static bool IsDown(int virtualKey) => (GetAsyncKeyState(virtualKey) & 0x8000) != 0;

        // Unity's legacy Input keeps reporting Alt as held after AltGr (Ctrl+Alt on Windows, e.g. '@'
        // on Turkish and German layouts) is released, which turned every later Backspace into
        // Alt+Backspace. Ask Windows for the physical key state instead.
        private static int CurrentModifiers()
        {
            int mods = 0;
            if (IsDown(0x10)) mods |= ModShift;              // VK_SHIFT
            if (IsDown(0x11)) mods |= ModCtrl;               // VK_CONTROL
            if (IsDown(0x12)) mods |= ModAlt;                // VK_MENU
            if (IsDown(0x5B) || IsDown(0x5C)) mods |= ModMeta; // VK_LWIN, VK_RWIN
            return mods;
        }
#else
        private static int CurrentModifiers()
        {
            int mods = 0;
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) mods |= ModShift;
            if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) mods |= ModCtrl;
            if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt) || Input.GetKey(KeyCode.AltGr)) mods |= ModAlt;
            if (Input.GetKey(KeyCode.LeftCommand) || Input.GetKey(KeyCode.RightCommand) ||
                Input.GetKey(KeyCode.LeftWindows) || Input.GetKey(KeyCode.RightWindows)) mods |= ModMeta;
            return mods;
        }
#endif

        private static readonly StringBuilder s_textBuilder = new StringBuilder(16);

        private static string FilterText(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;
            s_textBuilder.Length = 0;
            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];
                // Strips '\b', '\n', '\r', '\t' and Ctrl+letter codes; Backspace/Enter/Tab arrive via ptui_key.
                if (char.IsControl(c)) continue;
                s_textBuilder.Append(c);
            }
            return s_textBuilder.ToString();
        }

        private static KeyMapping[] BuildKeyMap()
        {
            var list = new System.Collections.Generic.List<KeyMapping>
            {
                new KeyMapping(KeyCode.Backspace, 0x08),
                new KeyMapping(KeyCode.Tab, 0x09),
                new KeyMapping(KeyCode.Return, 0x0D),
                new KeyMapping(KeyCode.KeypadEnter, 0x0D),
                new KeyMapping(KeyCode.Escape, 0x1B),
                new KeyMapping(KeyCode.Space, 0x20),
                new KeyMapping(KeyCode.PageUp, 0x21),
                new KeyMapping(KeyCode.PageDown, 0x22),
                new KeyMapping(KeyCode.End, 0x23),
                new KeyMapping(KeyCode.Home, 0x24),
                new KeyMapping(KeyCode.LeftArrow, 0x25),
                new KeyMapping(KeyCode.UpArrow, 0x26),
                new KeyMapping(KeyCode.RightArrow, 0x27),
                new KeyMapping(KeyCode.DownArrow, 0x28),
                new KeyMapping(KeyCode.Insert, 0x2D),
                new KeyMapping(KeyCode.Delete, 0x2E),
            };
            for (int i = 0; i < 10; i++) list.Add(new KeyMapping(KeyCode.Alpha0 + i, 0x30 + i));
            for (int i = 0; i < 26; i++) list.Add(new KeyMapping(KeyCode.A + i, 0x41 + i));
            for (int i = 0; i < 12; i++) list.Add(new KeyMapping(KeyCode.F1 + i, 0x70 + i));
            return list.ToArray();
        }

        // ---------------------------------------------------------------- teardown

        private void OnApplicationFocus(bool focused)
        {
            if (s_nativeAlive) PlaytoliaDesktopNative.playtolia_app_focus(focused ? 1 : 0);
        }

        private void OnApplicationQuit()
        {
            DestroyNative();
        }

        private void OnDestroy()
        {
            DestroyNative();
            ReleasePixels();
            if (_texture != null) Destroy(_texture);
            if (_material != null) Destroy(_material);
            if (s_instance == this) s_instance = null;
            PointerOverUi = false;
        }

        private void DestroyNative()
        {
            if (!s_nativeAlive || s_instance != this) return;
            s_nativeAlive = false;
            try
            {
                PlaytoliaDesktopNative.RemoveLogCallback();
                PlaytoliaDesktopNative.ptui_destroy();
            }
            catch (Exception e)
            {
                Debug.LogWarning(LogPrefix + "ptui_destroy threw: " + e.Message);
            }
        }
    }
}
