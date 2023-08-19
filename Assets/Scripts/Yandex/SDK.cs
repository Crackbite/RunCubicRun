using Agava.YandexGames;
using System;
using System.Collections;
using UnityEngine;

public class SDK : MonoBehaviour
{
    [SerializeField] private FailScreen _failScreen;
    [SerializeField] private SuccessScreen _succesScreen;

    private int _interstitialAdCounter;

    public event Action Initialized;
    public event Action AdOpened;
    public event Action AdClosed;

    private void OnEnable()
    {
        _failScreen.RefreshButtonClicked += OnRefreshButtonClicked;
        _succesScreen.NextButtonClicked += OnNextButtonClicked;
    }

    private void Awake()
    {
        YandexGamesSdk.CallbackLogging = true;
    }

    private void OnDisable()
    {
        _failScreen.RefreshButtonClicked -= OnRefreshButtonClicked;
        _succesScreen.NextButtonClicked -= OnNextButtonClicked;
    }

    public void ShowVideoAd()
    {
        VideoAd.Show(() => { AdOpened?.Invoke(); }, null, () => { AdClosed?.Invoke(); });
    }

    public void ShowInterstitialAd()
    {
        InterstitialAd.Show();
        _interstitialAdCounter++;
    }

    private IEnumerator Start()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        yield break;
#endif
        yield return YandexGamesSdk.Initialize(Initialized);
    }

    private void OnRefreshButtonClicked()
    {
        ShowVideoAd();
    }

    private void OnNextButtonClicked()
    {
        const int InterstitialAdCount = 3;

        if (_interstitialAdCounter >= InterstitialAdCount)
        {
            ShowVideoAd();
            _interstitialAdCounter -= _interstitialAdCounter;
            return;
        }

        ShowInterstitialAd();
    }
}
