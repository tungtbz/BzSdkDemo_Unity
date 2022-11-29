using System.Collections.Generic;
using RofiSdk;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Start()
    {
        RofiSdkHelper.Instance.NativeBridge.WarmUp();
        RofiSdkHelper.Instance.NativeBridge.SetDebugMode(true);
    }

    public void IsRewardVideoAvailable()
    {
        var available = RofiSdkHelper.Instance.NativeBridge.IsVideoRewardAvailable();
        Debug.Log("IsRewardVideoAvailable " + available);
    }

    public void ShowAds()
    {
        RofiSdkHelper.Instance.NativeBridge.ShowAds();
    }

    public void LogEvent1()
    {
        RofiSdkHelper.Instance.NativeBridge.LogEvent("Event_Name_1", new Dictionary<string, string>()
        {
            {"click_to_button","btnLogEvent"} 
        });
    }
    
    public void LogEvent2()
    {
        RofiSdkHelper.Instance.NativeBridge.LogEvent("Event_Name_2", new Dictionary<string, string>()
        {
            {"test_param_name","test_param_value"} 
        });
    }

    public void ShowLogin()
    {
        RofiSdkHelper.Instance.NativeBridge.OpenLoginScene();
    }
}
