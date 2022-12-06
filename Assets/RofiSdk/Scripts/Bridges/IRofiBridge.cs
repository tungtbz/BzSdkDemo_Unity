using System.Collections.Generic;
namespace RofiSdk
{
    public interface IRofiBridge
    {
        void WarmUp();
        bool IsVideoRewardAvailable();
        void ShowAds(string placementName = null);
        void LogEvent(string eventName, Dictionary<string, string> eventData);
        void SetDebugMode(bool isDebug);
        void OpenLoginScene();
        void GetUserInfo(string accessToken);
        void RefCheckIn(string accessToken, string gameId, string camId, string refCode);
        string GetRefCodeCached();
        string GetCurrentAccessToken();
        bool DeepLinkHandle(string url);
        void JoinCampaign(string accessToken, string campaignId);
    }
}

