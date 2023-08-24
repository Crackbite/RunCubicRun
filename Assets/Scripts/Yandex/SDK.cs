using Agava.YandexGames;
using System;
using System.Collections;
using UnityEngine;

public class SDK : MonoBehaviour
{
    [SerializeField] private FailScreen _failScreen;
    [SerializeField] private SuccessScreen _succesScreen;

    public event Action Initialized;
    public event Action AdOpened;
    public event Action AdClosed;

    private void OnEnable()
    {
        _failScreen.RefreshButtonClicked += OnRefreshButtonClicked;
        _succesScreen.NextLevelButtonClicked += OnNextButtonClicked;
    }

    private void Awake()
    {
        YandexGamesSdk.CallbackLogging = true;
    }

    private IEnumerator Start()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        yield break;
#endif
        yield return YandexGamesSdk.Initialize(Initialized);
    }

    private void OnDisable()
    {
        _failScreen.RefreshButtonClicked -= OnRefreshButtonClicked;
        _succesScreen.NextLevelButtonClicked -= OnNextButtonClicked;
    }

    public void ShowVideoAd()
    {
        VideoAd.Show(OnAdOpened, null, OnAdClosed);
    }

    public void ShowInterstitialAd()
    {
        InterstitialAd.Show(OnAdOpened, (bool _) => { AdClosed?.Invoke(); });
    }

    private void OnAdOpened()
    {
        AdOpened?.Invoke();
    }

    private void OnAdClosed()
    {
        AdClosed?.Invoke();
    }

    private void OnRefreshButtonClicked()
    {
        ShowVideoAd();
    }

    private void OnNextButtonClicked(int currentLevel)
    {
        const int LevelsBetweenVideoAd = 3;
        const int TargetRemainder = 0;
        const int MinLevelCount = 1;

        if (currentLevel >= MinLevelCount && currentLevel % LevelsBetweenVideoAd == TargetRemainder)
        {
            ShowVideoAd();
            return;
        }

        ShowInterstitialAd();
    }
}