using System.Collections.Generic;

namespace RofiSdk
{
    public class EditorDummyBridge : IRofiBridge
    {
        public void WarmUp()
        {
            
        }

        public bool IsVideoRewardAvailable()
        {
            return false;
        }

        public void ShowAds(string placementName = null)
        {

        }

        public void LogEvent(string eventName, Dictionary<string, string> eventData)
        {
          
        }

        public void SetDebugMode(bool isDebug)
        {
    
        }

        public void OpenLoginScene()
        {
            PopupManager.Instance.OpenPopup(new OpenPopupSetting()
            {
                popupPrefabPath = "LoginPopup", type = PopupType.FULL_SCREEN
            });
        }

        public void GetUserInfo(string accessToken)
        {
          
        }

        public void RefCheckIn(string accessToken, string refCode)
        {
         
        }

        public string GetRefCodeCached()
        {
            return "";
        }

        public string GetCurrentAccessToken()
        {
            return "";
        }

        public bool DeepLinkHandle(string url)
        {
            return false;
        }

        public void JoinCampaign(string accessToken)
        {
        }
    }
}