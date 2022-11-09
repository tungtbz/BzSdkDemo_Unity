namespace BzSdk
{
#if UNITY_IOS
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class BzIOSBridge : PersistentSingleton<BzIOSBridge>
    {
        [DllImport ("__Internal")] 
        private static extern bool _WarmUp();
        [DllImport ("__Internal")] 
        private static extern bool _IsRewardAvailable();
        [DllImport ("__Internal")] 
        private static extern void _ShowAds();
        [DllImport ("__Internal")]
        private static extern void _ShowAdsWithPlacement(string placementName);
        [DllImport ("__Internal")]
        private static extern void _LogEvent(string eventName, string eventData);
        
        protected override void Awake()
        {
            base.Awake();
            gameObject.name = "BzSDKEvent";
        }

        public void Warmup()
        {
            _WarmUp();
        }

        public bool IsVideoRewardAvailable()
        {
            return _IsRewardAvailable();
        }

        public void ShowAds(string placementName = null)
        {
            if(placementName == null) _ShowAds();
            else _ShowAdsWithPlacement(placementName);
        }

        public void LogEvent(string eventName, Dictionary<string, string> eventData)
        {
            _LogEvent(eventName, SimpleJson.Serialize(eventData));
        }
        
        public void onRewardedVideoAdRewarded(string description)
        {
            Debug.Log("~~~~~~ onRewardedVideoAdRewarded");
        }
    }
#endif
}
