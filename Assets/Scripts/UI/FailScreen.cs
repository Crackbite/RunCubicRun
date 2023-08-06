using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class FailScreen : LevelResultScreen
{
    [SerializeField] private Button _restart;
    [SerializeField] private Button _refresh;
    [SerializeField] private float _maxWindowDelay = 1.5f;
    [SerializeField] private DOTweenAnimation _windowAnimation;
    [SerializeField] private DOTweenAnimation _containerAnimation;

    public event Action LevelRestarting;

    protected override void OnEnable()
    {
        base.OnEnable();
        _restart.onClick.AddListener(OnRestartClicked);

        if (IsTraining)
        {
            _refresh.interactable = false;
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _restart.onClick.RemoveListener(OnRestartClicked);
    }

    public void Enter(GameResult gameResult)
    {
        switch (gameResult)
        {
            case GameResult.LoseWithBlocksEnded:
            case GameResult.LoseWithPortalSuckedIn:
                _windowAnimation.delay = 0f;
                break;
            case GameResult.LoseWithHit:
            default:
                _windowAnimation.delay = _maxWindowDelay;
                break;
        }

        _containerAnimation.delay = _windowAnimation.delay + _windowAnimation.duration;
        base.Enter();
    }

    protected override void RestartLevel()
    {
        LevelRestarting?.Invoke();
    }

    private void OnRestartClicked()
    {
        if (ChunkStorage.Instance != null)
        {
            ChunkStorage.Instance.Restart();
        }

        LoadScene();
    }
}