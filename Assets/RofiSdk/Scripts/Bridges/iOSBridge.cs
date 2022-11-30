namespace RofiSdk
{
#if UNITY_IOS
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class iOSBridge : IRofiBridge
    {
        [DllImport("__Internal")]
        private static extern bool _WarmUp();
        [DllImport("__Internal")]
        private static extern bool _IsRewardAvailable();
        [DllImport("__Internal")]
        private static extern void _ShowAds();
        [DllImport("__Internal")]
        private static extern void _ShowAdsWithPlacement(string placementName);
        [DllImport("__Internal")]
        private static extern void _LogEvent(string eventName, string eventData);
        [DllImport("__Internal")]
        private static extern void _OpenLoginScene();
        [DllImport("__Internal")]
        private static extern void _SetDebugMode(bool isDebug);

        public void WarmUp()
        {
            _WarmUp();
        }

        public bool IsVideoRewardAvailable()
        {
            return _IsRewardAvailable();
        }

        public void ShowAds(string placementName = null)
        {
            if (placementName == null) _ShowAds();
            else _ShowAdsWithPlacement(placementName);
        }

        public void LogEvent(string eventName, Dictionary<string, string> eventData)
        {
            _LogEvent(eventName, SimpleJson.Serialize(eventData));
        }

        public void OpenLoginScene()
        {
            _OpenLoginScene();
        }

        public void GetUserInfo(string accessToken)
        {
            
        }

        public void GetUserInfo()
        {
            
        }

        public void SetDebugMode(bool isDebug)
        {
            _SetDebugMode(isDebug);
        }
    }
#endif
}
