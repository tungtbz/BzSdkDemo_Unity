namespace RofiSdk
{
    using UnityEngine;

    public class RofiSdkHelper : PersistentSingleton<RofiSdkHelper>
    {
        IRofiBridge _rofiBridge;
        private string deeplinkURL;

        protected override void Awake()
        {
            base.Awake();
            gameObject.name = RofiConstants.SDK_OBJECT_NAME;
#if PLATFORM_IOS
            _rofiBridge = new iOSBridge();
#elif UNITY_ANDROID
            _rofiBridge = new AndroidBridge();
#endif
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

        private void ApplicationOndeepLinkActivated(string url)
        {
            Debug.Log("url: " + url);
            deeplinkURL = url;
            var isDeepLinkValid = _rofiBridge.DeepLinkHandle(deeplinkURL);
            if (isDeepLinkValid)
            {
                Debug.Log("OndeepLinkActivated");
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
        }

        public void OnGetUserInfo(string data)
        {
            Debug.Log("[Unity] GetUserInfo " + data);
        }

        public void OnGetUserInfoFailed(string mesage)
        {
            Debug.Log("[Unity] OnGetUserInfoFailed " + mesage);
        }

        public void OnRefCheckInSuccess()
        {
            Debug.Log("[Unity] OnRefCheckInSuccess ");
        }

        public void OnRefCheckInFail(string message)
        {
            Debug.Log("[Unity] OnGetUserInfoFailed " + message);
        }

        #endregion
    }
}