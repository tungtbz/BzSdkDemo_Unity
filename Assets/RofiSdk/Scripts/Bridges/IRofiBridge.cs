﻿using System.Collections.Generic;
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
    }
}

