using System;
using UnityEngine;
using UnityEngine.UI;

public class SuccessScreen : LevelResultScreen
{
    [SerializeField] private Button _next;

    private bool _isStartWithoutMenu;

    public event Action<int> NextLevelButtonClicked;

    protected override void OnEnable()
    {
        base.OnEnable();
        _next.onClick.AddListener(OnNextClicked);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _next.onClick.RemoveListener(OnNextClicked);
    }

    protected override void OnHomeClicked()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        LoadScene();
        return;
#endif

        NextLevelButtonClicked?.Invoke(CurrentLevel);
    }

    protected override void OnAdClosed()
    {
        if (enabled)
        {
            LoadScene(_isStartWithoutMenu);
        }
    }

    private void OnNextClicked()
    {
        _isStartWithoutMenu = true;

#if !UNITY_WEBGL || UNITY_EDITOR
        LoadScene(_isStartWithoutMenu);
        return;
#endif

        NextLevelButtonClicked?.Invoke(CurrentLevel);
    }
}