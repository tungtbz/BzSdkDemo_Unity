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
            _javaBridge.CallStatic("WarmUp");
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
            _javaBridge.CallStatic("SetDebug", isDebug);
        }

        public void LogEvent(string eventName, Dictionary<string, string> eventData)
        {
            Debug.Log("AndroidBridge log event: " + SimpleJson.Serialize(eventData));
            _javaBridge.CallStatic("LogEvent", eventName, SimpleJson.Serialize(eventData));
        }

        public void OpenLoginScene()
        {
            _javaBridge.CallStatic("OpenLoginScene");
        }

        public void GetUserInfo(string accessToken)
        {
            _javaBridge.CallStatic("GetUserInfo", accessToken);
        }

        public void RefCheckIn(string accessToken, string gameId, string camId, string refCode)
        {
            Debug.Log("[Unity] AndroidBridge: RefCheckIn");
            
            _javaBridge.CallStatic("RefCheckIn", accessToken, gameId, camId, refCode);
        }

        public string GetRefCodeCached()
        {
            return _javaBridge.CallStatic<string>("GetRefCodeCached");
        }

        public string GetCurrentAccessToken()
        {
            return _javaBridge.CallStatic<string>("GetCurrentAccessToken");
        }

        public bool DeepLinkHandle(string url)
        {
            return true;
        }
    }
}