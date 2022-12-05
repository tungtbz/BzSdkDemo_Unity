namespace RofiSdk
{
#if UNITY_IOS
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class iOSBridge : IRofiBridge
    {
        [DllImport("__Internal")]
        private static extern string _GetRefCodeCached();
        [DllImport("__Internal")]
        private static extern string _SetRefCodeCached(string refCode);
        
        [DllImport("__Internal")]
        private static extern string _GetCurrentAccessToken();
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

        [DllImport("__Internal")]
        private static extern void _GetUserInfo(string accessToken);

        [DllImport("__Internal")]
        private static extern void _RefCheckIn(string accessToken, string gameId, string camId, string refCode);

        private string _cacheRefCode;
        
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
            _GetUserInfo(accessToken);
        }

        public void RefCheckIn(string accessToken, string gameId, string camId, string refCode)
        {
            _RefCheckIn(accessToken, gameId, camId, refCode);
        }

        public string GetRefCodeCached()
        {
            return _cacheRefCode;
        }

        public string GetCurrentAccessToken()
        {
            return _GetCurrentAccessToken();
        }
        
        //    idolworlddl://invite.rofi.games/referral/HSHSHS
        public bool DeepLinkHandle(string url)
        {
            var splitUrl = url.Split(':');
            if (!splitUrl[0].Equals("idolworlddl"))
            {
                return false;
            }
            Debug.Log("DeepLinkHandle: splitUrl[1]: " + splitUrl[1]);
            var deppLinkParts = splitUrl[1].Split('/');
            foreach (var str in deppLinkParts)
            {
                Debug.Log("DeepLinkHandle: " + str);
            }

            var partCount = deppLinkParts.Length;
            var deeplinkType = deppLinkParts[partCount - 2];
            if (deeplinkType.Equals("referral"))
            {
                var refCode = deppLinkParts[partCount - 1];
                Debug.Log("DeepLinkHandle --> referral --> refCode: " + refCode);
                // _SetRefCodeCached(refCode);
                _cacheRefCode = refCode;
                return true;
            }
            return false;
        }

        public void SetDebugMode(bool isDebug)
        {
            _SetDebugMode(isDebug);
        }
    }

#endif
}