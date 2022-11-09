using System;
using System.Collections.Generic;
using BzSdk;
using UnityEngine;

public class Test : MonoBehaviour
{
#if UNITY_IOS
    private void Start()
    {
        BzIOSBridge.Instance.Warmup();
    }

    public void IsRewardVideoAvailable()
    {
        var available = BzIOSBridge.Instance.IsVideoRewardAvailable();
        Debug.Log("IsRewardVideoAvailable " + available);
    }

    public void ShowAds()
    {
        BzIOSBridge.Instance.ShowAds();
    }

    public void LogEvent1()
    {
        BzIOSBridge.Instance.LogEvent("Event_Name_1", new Dictionary<string, string>()
        {
            {"click_to_button","btnLogEvent"} 
        });
    }
    
    public void LogEvent2()
    {
        BzIOSBridge.Instance.LogEvent("Event_Name_2", new Dictionary<string, string>()
        {
            {"test_param_name","test_param_value"} 
        });
    }
#endif
}
