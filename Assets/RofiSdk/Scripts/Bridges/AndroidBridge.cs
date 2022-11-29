namespace RofiSdk
{
    using System.Collections.Generic;
    using UnityEngine;
    public class AndroidBridge : IRofiBridge
    {
        readonly AndroidJavaClass _javaBridge;
        public AndroidBridge()
        {
            _javaBridge = new AndroidJavaClass(RofiConstants.ANDROID_BRIDGE_CLASS);
        }

        public void WarmUp()
        {
        }

        public bool IsVideoRewardAvailable()
        {
            return _javaBridge.CallStatic<bool>("IsRewardedVideoAvailable");
        }

        public void ShowAds(string placementName = null)
        {
            _javaBridge.CallStatic("ShowVideoReward");
        }

        public void SetDebugMode(bool isDebug)
        {

        }

        public void LogEvent(string eventName, Dictionary<string, string> eventData)
        {
            _javaBridge.CallStatic("LogEventOpenApp");
        }

        public void OpenLoginScene()
        {
            _javaBridge.CallStatic("OpenLoginScene");
        }
    }
}

