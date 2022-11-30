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
        // RofiSdkHelper.Instance.NativeBridge.LogEvent("Event_Name_1", new Dictionary<string, string>()
        // {
        //     {"click_to_button","btnLogEvent"} 
        // });
        RofiSdkHelper.Instance.NativeBridge.GetUserInfo("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1aWQiOiIxNDZiN2VlNi00ODgxLTRmM2ItYWIyMi1kNmU5Y2I5N2ViM2UiLCJlbWFpbCI6IiIsIndhbGxldCI6IiIsIm5iZiI6MTY2OTc4NjgyMiwiZXhwIjoxNjY5ODIyODIyLCJpYXQiOjE2Njk3ODY4MjIsImlzcyI6ImlkLnJvZmkuZ2FtZXMiLCJhdWQiOiJhcGkucm9maS5nYW1lcyJ9.eXZ-THGv0x-ZYji31t3RZdseG6Q5ocPffAXYP3HZ4g0");
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
