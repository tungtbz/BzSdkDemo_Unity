namespace RofiSdk
{
    using UnityEngine;

    public class RofiSdkHelper : PersistentSingleton<RofiSdkHelper>
    {
        IRofiBridge _rofiBridge;

        protected override void Awake()
        {
            base.Awake();
            gameObject.name = RofiConstants.SDK_OBJECT_NAME;
#if PLATFORM_IOS
            _rofiBridge = new iOSBridge();
#elif UNITY_ANDROID
            _rofiBridge = new AndroidBridge();
#endif
        }

        public IRofiBridge NativeBridge => _rofiBridge;

        #region callback from native

        //

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