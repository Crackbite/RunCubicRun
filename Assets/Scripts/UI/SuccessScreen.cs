using System;
using UnityEngine;
using UnityEngine.UI;

public class SuccessScreen : LevelResultScreen
{
    [SerializeField] private Button _next;
    [SerializeField] private SDK _sdk;

    private bool _isStartWithoutMenu;

    public event Action<int> NextLevelButtonClicked;

    protected override void OnEnable()
    {
        base.OnEnable();
        _next.onClick.AddListener(OnNextClicked);
        _sdk.AdClosed += OnAdClosed;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _next.onClick.RemoveListener(OnNextClicked);
        _sdk.AdClosed -= OnAdClosed;
    }

    protected override void OnHomeClicked()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        LoadScene();
        return;
#endif

        NextLevelButtonClicked?.Invoke(CurrentLevel);
    }

    private void OnNextClicked()
    {
        if (ChunkStorage.Instance != null)
        {
            ChunkStorage.Instance.Restart();
        }

        _isStartWithoutMenu = true;

#if !UNITY_WEBGL || UNITY_EDITOR
        LoadScene(_isStartWithoutMenu);
        return;
#endif

        NextLevelButtonClicked?.Invoke(CurrentLevel);
    }

    private void OnAdClosed()
    {
        LoadScene(_isStartWithoutMenu);
    }
}