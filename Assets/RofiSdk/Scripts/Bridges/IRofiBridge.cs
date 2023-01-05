using System.Collections.Generic;
namespace RofiSdk
{
    public interface IRofiBridge
    {
        void WarmUp();
        bool IsVideoRewardAvailable();
        void ShowAds(string placementName = null);
        bool IsInterAdsAvailable();
        void ShowInterAds(int requestCode);
        void ShowVideoAds(string placement = null, int requestCode = 0);
        void LogEvent(string eventName, Dictionary<string, string> eventData);
        void SetDebugMode(bool isDebug);
        void SetMode(int mode);
        void OpenLoginScene();
        void GetUserInfo(string accessToken);
        void RefCheckIn(string accessToken, string refCode);
        string GetRefCodeCached();
        string GetCurrentAccessToken();
        bool DeepLinkHandle(string url);
        void JoinCampaign(string accessToken);
    }
}

