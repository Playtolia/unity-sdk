using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.Android;
using System;
#if PLAYTOLIA_FIREBASE
using Firebase.Extensions;
#endif
using PlaytoliaSDK.Runtime;

public class PlaytoliaGameObject : MonoBehaviour
{
#if UNITY_IPHONE
    // On iOS plugins are statically linked into
    // the executable, so we have to use __Internal as the
    // library name.
    [DllImport("__Internal")]
    private static extern void InitializePlaytolia();
#endif

    // Firebase-backed push is optional. The PLAYTOLIA_FIREBASE define is managed by
    // Playtolia Settings (it's set when "Enable Push Notifications" is on), so projects
    // without the Firebase Unity SDK compile fine and simply skip push registration.
#if PLAYTOLIA_FIREBASE
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs token)
    {
        UnityEngine.Debug.Log("Received Registration Token: " + token.Token);
        PlaytoliaNotifications.RegisterPushToken(token.Token);
    }

    public void OnMessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e)
    {
        UnityEngine.Debug.Log("Received a new message from: " + e.Message.From);
    }
#endif

    void Start()
    {
#if PLAYTOLIA_FIREBASE
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                Debug.Log("PlaytoliaGameObject: Firebase dependencies are available.");
                Debug.Log("PlaytoliaGameObject: Initializing Firebase Messaging...");
                Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
                Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;

                // Actively fetch token — OnTokenReceived only fires on generation/refresh,
                // not on every app launch if token is already cached.
                Firebase.Messaging.FirebaseMessaging.GetTokenAsync().ContinueWithOnMainThread(tokenTask =>
                {
                    if (tokenTask.IsCompletedSuccessfully && !string.IsNullOrEmpty(tokenTask.Result))
                    {
                        Debug.Log("PlaytoliaGameObject: Fetched existing FCM token.");
                        PlaytoliaNotifications.RegisterPushToken(tokenTask.Result);
                    }
                });
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                    "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
#else
        Debug.Log("PlaytoliaGameObject: Push notifications disabled " +
                  "(PLAYTOLIA_FIREBASE not defined); skipping Firebase Messaging initialization.");
#endif

        switch (Application.platform)
        {
            case RuntimePlatform.IPhonePlayer:
                Debug.Log("PlaytoliaGameObject: Initializing Playtolia for iOS");
#if UNITY_IPHONE
                InitializePlaytolia();
#endif
                break;
            case RuntimePlatform.Android:
                Debug.Log("PlaytoliaGameObject: Initializing Playtolia for Android");
                // co.xreos.playtoliasdk.ui.PlaytoliaUI.INSTANCE.attachToUnity("TAG");

                try
                {
                    AndroidJavaClass playtoliaClass = new AndroidJavaClass("com.playtolia.sdk.ui.PlaytoliaUI");

                    AndroidJavaObject playtoliaInstance = playtoliaClass.GetStatic<AndroidJavaObject>("INSTANCE");
                    if (playtoliaInstance == null)
                    {
                        Debug.LogError("PlaytoliaGameObject: Playtolia instance not found");
                        return;
                    }

                    playtoliaInstance.Call("attachToUnity", "default");
                    Debug.Log("PlaytoliaGameObject: Playtolia initialized successfully for Android");
                }
                catch (Exception e)
                {
                    Debug.LogError("PlaytoliaGameObject: Failed to initialize Playtolia for Android: " + e.Message);
                    Debug.LogException(e);
                }

                break;
            default:
                Debug.LogWarning("PlaytoliaGameObject: Unsupported platform for Playtolia initialization");
                break;
        }
    }

    void OnComponentStateChange(string componentName)
    {
        Debug.Log("PlaytoliaGameObject: Component state changed: " + componentName +
                  ". Notifying PlaytoliaLibInterop for further processing.");
        PlaytoliaLibInterop.NotifyComponentStateChanges(componentName);
    }

    void OnInteropBridgeMessage(string json)
    {
        Debug.Log("PlaytoliaGameObject: Received bridge message: " + json);
        InteropBridge.HandleMessage(json);
    }
}