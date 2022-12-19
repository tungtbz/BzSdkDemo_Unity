

namespace RofiSdk
{
#if UNITY_EDITOR
    using Sirenix.OdinInspector;
#endif
    using TinyIoC;
    using TinyMessenger;

    using UnityEngine;

    public class RofiSdkCallbackMessage : GenericTinyMessage<string>
    {
        public RofiSdkCallbackMessage(object sender, string content) : 
            base(sender, content)
        {
        }
    }

    public class RofiSdkHelper : PersistentSingleton<RofiSdkHelper>
    {
        IRofiBridge _rofiBridge;
        private string deeplinkURL;
        private TinyIoCContainer _iocContainer;
        
        public void Init()
        {
            gameObject.name = RofiConstants.SDK_OBJECT_NAME;
            _iocContainer = TinyIoCContainer.Current;
            SetupPopup();
            SetupNativeBridge();

            Application.deepLinkActivated += ApplicationOndeepLinkActivated;
            if (!string.IsNullOrEmpty(Application.absoluteURL))
            {
                // Cold start and Application.absoluteURL not null so process Deep Link.
                Debug.Log("Application Cold start from deeplink");
                ApplicationOndeepLinkActivated(Application.absoluteURL);
            }
            // Initialize DeepLink Manager global variable.
            else deeplinkURL = "[none]";
        }

        private void SetupPopup()
        {
            // container.Register<PopupManager>().AsSingleton();
            // _iocContainer.Register<PopupManager>(PopupManager.Instance);
            SimpleObjectPool.Spawn("PopupCanvas");
        }

        private void SetupNativeBridge()
        {
            var container = TinyIoCContainer.Current;
#if UNITY_EDITOR
           container.Register<IRofiBridge>( new EditorDummyBridge());
#elif UNITY_IOS
            container.Register<IRofiBridge>( new iOSBridge());
#elif UNITY_ANDROID
            container.Register<IRofiBridge>(new AndroidBridge());
#endif
            _rofiBridge = container.Resolve<IRofiBridge>();
        }

        public ITinyMessengerHub MessageHub => _iocContainer.Resolve<ITinyMessengerHub>();  

        private void ApplicationOndeepLinkActivated(string url)
        {
            Debug.Log("url: " + url);
            deeplinkURL = url;
            var isDeepLinkValid = _rofiBridge.DeepLinkHandle(deeplinkURL);
            if (isDeepLinkValid)
            {
                Debug.Log("OndeepLinkActivated");
                MessageHub.Publish(new RofiSdkCallbackMessage(this, "Referral Code:" + _rofiBridge.GetRefCodeCached()));
            }
            else
            {
                MessageHub.Publish(new RofiSdkCallbackMessage(this, url));
            }
        }

        public IRofiBridge NativeBridge => _rofiBridge;

#region callback from native

        public void OnVideoAdFailed(string description)
        {
            Debug.Log("[Unity] OnVideoAdFailed " + description);
        }

        public void OnVideoAdRewarded(string placement)
        {
            Debug.Log("[Unity] OnAdsComplete " + placement);
        }

        public void OnLoginInComplete(string data)
        {
            Debug.Log("[Unity] OnLoginComplete " + data);
            MessageHub.Publish(new RofiSdkCallbackMessage(this,data));
        }

        public void OnGetUserInfo(string data)
        {
            Debug.Log("[Unity] GetUserInfo " + data);
            MessageHub.Publish(new RofiSdkCallbackMessage(this,data));
        }

        public void OnGetUserInfoFailed(string mesage)
        {
            Debug.Log("[Unity] OnGetUserInfoFailed " + mesage);
            MessageHub.Publish(new RofiSdkCallbackMessage(this,mesage));
        }

        public void OnRefCheckInSuccess()
        {
            Debug.Log("[Unity] OnRefCheckInSuccess ");
            MessageHub.Publish(new RofiSdkCallbackMessage(this,"OnRefCheckInSuccess"));
        }

        public void OnRefCheckInFail(string message)
        {
            Debug.Log("[Unity] OnGetUserInfoFailed " + message);
            MessageHub.Publish(new RofiSdkCallbackMessage(this,message));
        }

        public void OnGetRefDataSuccess(string data)
        {
            Debug.Log("[Unity] OnGetRefDataSuccess " + data);
            MessageHub.Publish(new RofiSdkCallbackMessage(this,data));
        }

        public void OnGetRefDataFail(string message)
        {
            Debug.Log("[Unity] OnGetRefDataFail " + message);  
            MessageHub.Publish(new RofiSdkCallbackMessage(this,message));
        }

        #endregion
#if UNITY_EDITOR
        [Button]
        private void ShowLoginPopup()
        {

        }
#endif
    }
}