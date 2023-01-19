using System;
using System.Collections.Generic;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;

public class AnalyticsManager : MonoBehaviour
{
    private void Awake()
    {
        UnityServices.InitializeAsync();
    }

    public void SendLevelStart(int levelIndex)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("Level Start", levelIndex);
     
        AnalyticsService.Instance.CustomData("Level Start Event" ,data);
        AnalyticsService.Instance.Flush();
    }
}
