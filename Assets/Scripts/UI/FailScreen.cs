using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FailScreen : LevelResultScreen
{
    [SerializeField] private Button _home;
    [SerializeField] private float _maxWindowDelay = 1.5f;
    [SerializeField] private DOTweenAnimation _windowAnimation;
    [SerializeField] private DOTweenAnimation _containerAnimation;

    protected override void OnEnable()
    {
        base.OnEnable();
        _home.onClick.AddListener(OnHomeClicked);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _home.onClick.RemoveListener(OnHomeClicked);
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


    private void OnHomeClicked()
    {
        if (ChunkStorage.Instance != null)
        {
            ChunkStorage.Instance.Restart();
        }

        LoadScene();
    }
}