using System.Collections.Generic;
using RofiSdk.Models;
using TinyMessenger;

namespace RofiSdk
{
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
        private TinyMessengerHub _messengerHub;
        protected override void Awake()
        {
            base.Awake();
            _messengerHub = new TinyMessengerHub();
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

        public TinyMessengerHub MessageHub => _messengerHub;

        private void ApplicationOndeepLinkActivated(string url)
        {
            Debug.Log("url: " + url);
            deeplinkURL = url;
            var isDeepLinkValid = _rofiBridge.DeepLinkHandle(deeplinkURL);
            if (isDeepLinkValid)
            {
                Debug.Log("OndeepLinkActivated");
                _messengerHub.Publish(new RofiSdkCallbackMessage(this, "Referral Code:" + _rofiBridge.GetRefCodeCached()));
            }
            else
            {
                _messengerHub.Publish(new RofiSdkCallbackMessage(this, url));
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

        public void OnVideoRewardedWithCode(string placement, int code)
        {
            Debug.Log("[Unity] OnAdsComplete " + placement + " code: " + code);
            _messengerHub.Publish(new AdsCallback(this) {requestCode =  code});
        }

        public void OnLoginInComplete(string data)
        {
            Debug.Log("[Unity] OnLoginComplete " + data);
            _messengerHub.Publish(new RofiSdkCallbackMessage(this,data));
        }

        public void OnGetUserInfo(string data)
        {
            Debug.Log("[Unity] GetUserInfo " + data);
            _messengerHub.Publish(new RofiSdkCallbackMessage(this,data));
        }

        public void OnGetUserInfoFailed(string mesage)
        {
            Debug.Log("[Unity] OnGetUserInfoFailed " + mesage);
            _messengerHub.Publish(new RofiSdkCallbackMessage(this,mesage));
        }

        public void OnRefCheckInSuccess()
        {
            Debug.Log("[Unity] OnRefCheckInSuccess ");
            _messengerHub.Publish(new RofiSdkCallbackMessage(this,"OnRefCheckInSuccess"));
        }

        public void OnRefCheckInFail(string message)
        {
            Debug.Log("[Unity] OnGetUserInfoFailed " + message);
            _messengerHub.Publish(new RofiSdkCallbackMessage(this,message));
        }

        public void OnGetRefDataSuccess(string data)
        {
            Debug.Log("[Unity] OnGetRefDataSuccess " + data);
            _messengerHub.Publish(new RofiSdkCallbackMessage(this,data));
           
            Dictionary<string,object> referralResponse =(Dictionary<string,object>) SimpleJson.Deserialize(data);
           // _messengerHub.Publish();
           Debug.Log("Unity: " + (string) referralResponse["code"]);
        }

        public void OnGetRefDataFail(string message)
        {
            Debug.Log("[Unity] OnGetRefDataFail " + message);  
            _messengerHub.Publish(new RofiSdkCallbackMessage(this,message));
        }

        #endregion
    }
}