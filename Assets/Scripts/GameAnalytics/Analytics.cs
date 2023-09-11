using GameAnalyticsSDK;
using UnityEngine;

public class Analytics : MonoBehaviour
{
    private void Awake()
    {
        GameAnalytics.Initialize();
    }
}
