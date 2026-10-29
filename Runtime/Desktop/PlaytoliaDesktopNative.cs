using System;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

namespace Playtolia.Desktop
{
    /// <summary>
    /// P/Invoke surface of the desktop SDK library (playtolia.dll). See desktop/README.md.
    /// All calls must happen on Unity's main thread.
    /// </summary>
    internal static class PlaytoliaDesktopNative
    {
        internal const string Lib = "playtolia";

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void LogFn(int level, IntPtr utf8);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int ptui_create(int width, int height, float density);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void ptui_resize(int width, int height, float density);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void ptui_destroy();

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int ptui_render(long nanoTime, IntPtr pixels, int force);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void ptui_pointer(int eventType, float x, float y, int button, int buttons,
            float scrollX, float scrollY, int modifiers);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int ptui_key(int type, int keyCode, int modifiers);

        // byte[] is blittable: the marshaller pins it and passes a pointer to the first element.
        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ptui_text(byte[] utf8NulTerminated);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int ptui_hit_test(float x, float y);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int ptui_wants_keyboard();

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr ptui_last_error();

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ptui_set_log_callback(LogFn fn);

        // Kept in a static field so the GC never collects the delegate the native side holds.
        private static LogFn s_logCallback;

        internal static void SendText(string text)
        {
            if (string.IsNullOrEmpty(text)) return;
            int len = Encoding.UTF8.GetByteCount(text);
            var bytes = new byte[len + 1];
            Encoding.UTF8.GetBytes(text, 0, text.Length, bytes, 0);
            bytes[len] = 0;
            ptui_text(bytes);
        }

        internal static string LastError()
        {
            return PtrToStringUtf8(ptui_last_error());
        }

        internal static void InstallLogCallback()
        {
            if (s_logCallback == null) s_logCallback = OnNativeLog;
            ptui_set_log_callback(s_logCallback);
        }

        internal static void RemoveLogCallback()
        {
            // The delegate's native thunk dies with the domain (Editor domain reload), so clear it.
            ptui_set_log_callback(null);
        }

        [AOT.MonoPInvokeCallback(typeof(LogFn))]
        private static void OnNativeLog(int level, IntPtr utf8)
        {
            string msg;
            try
            {
                msg = "[Playtolia] " + PtrToStringUtf8(utf8);
            }
            catch (Exception)
            {
                return;
            }

            switch (level)
            {
                case 3: Debug.LogError(msg); break;
                case 2: Debug.LogWarning(msg); break;
                default: Debug.Log(msg); break;
            }
        }


        // ------------------------------------------------------------ SDK lifecycle

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void SendMessageFn(IntPtr gameObject, IntPtr method, IntPtr message);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        private static extern void playtolia_set_send_message(SendMessageFn fn);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        private static extern int playtolia_attach(byte[] configJsonUtf8);

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int playtolia_status();

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void playtolia_tick();

        [DllImport(Lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void playtolia_app_focus(int focused);

        private static SendMessageFn s_sendMessage;

        // The SDK may call back from any thread; Unity APIs are main-thread only, so messages are
        // queued here and delivered by DrainMessages() from the overlay's Update, like
        // UnitySendMessage does on iOS.
        private static readonly System.Collections.Concurrent.ConcurrentQueue<string[]> s_messages =
            new System.Collections.Concurrent.ConcurrentQueue<string[]>();

        internal static bool Attach(string configJson)
        {
            // Before attach, so the SDK's startup logs reach the player log too.
            InstallLogCallback();
            if (s_sendMessage == null) s_sendMessage = OnSendMessage;
            playtolia_set_send_message(s_sendMessage);
            return playtolia_attach(Utf8(configJson)) == 1;
        }

        internal static void DrainMessages()
        {
            while (s_messages.TryDequeue(out var m))
            {
                var target = GameObject.Find(m[0]);
                if (target == null)
                {
                    Debug.LogWarning("[Playtolia] message for missing GameObject '" + m[0] + "': " + m[1]);
                    continue;
                }
                target.SendMessage(m[1], m[2], SendMessageOptions.DontRequireReceiver);
            }
        }

        [AOT.MonoPInvokeCallback(typeof(SendMessageFn))]
        private static void OnSendMessage(IntPtr gameObject, IntPtr method, IntPtr message)
        {
            try
            {
                s_messages.Enqueue(new[] { PtrToStringUtf8(gameObject), PtrToStringUtf8(method), PtrToStringUtf8(message) ?? "" });
            }
            catch (Exception)
            {
                // Never let an exception unwind into native code.
            }
        }

        internal static byte[] Utf8(string text)
        {
            text = text ?? string.Empty;
            int len = Encoding.UTF8.GetByteCount(text);
            var bytes = new byte[len + 1];
            Encoding.UTF8.GetBytes(text, 0, text.Length, bytes, 0);
            bytes[len] = 0;
            return bytes;
        }

        /// <summary>Marshal.PtrToStringUTF8 is not available on every profile, so scan for NUL.</summary>
        internal static string PtrToStringUtf8(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero) return null;
            int len = 0;
            while (Marshal.ReadByte(ptr, len) != 0) len++;
            if (len == 0) return string.Empty;
            var buffer = new byte[len];
            Marshal.Copy(ptr, buffer, 0, len);
            return Encoding.UTF8.GetString(buffer);
        }
    }
}
