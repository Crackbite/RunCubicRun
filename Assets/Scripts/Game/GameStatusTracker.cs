using IJunior.TypedScenes;
using System;
using UnityEngine;

public class GameStatusTracker : MonoBehaviour, ISceneLoadHandler<LevelLoadConfig>
{
    [SerializeField] private Cubic _cubic;
    [SerializeField] private CubicMovement _cubicMovement;
    [SerializeField] private BlockStack _blockStack;
    [SerializeField] private LevelExitPortal _exitPortal;
    [SerializeField] private MenuScreen _menuScreen;
    [SerializeField] private DataRestorer _dataRestorer;

    private bool _isCubicLeftPress;
    private bool _isCubicSteppedOnStand;

    public event Action<GameResult> GameEnded;
    public event Action GameStarted;

    public bool IsStartWithoutMenu { get; private set; }

    private void OnEnable()
    {
        _cubic.Hit += OnCubicHit;
        _cubic.SteppedOnStand += OnCubicSteppedOnStand;
        _cubicMovement.CubicLeftPress += OnCubicLeftPress;
        _blockStack.BlocksEnded += OnBlockStackBlocksEnded;
        _exitPortal.SuckedIn += OnExitPortalSuckedIn;
        _menuScreen.StartClicked += OnMenuStartClicked;
        _dataRestorer.DataRestored += OnGamaDataRestored;
    }

    private void OnDisable()
    {
        _cubic.Hit -= OnCubicHit;
        _cubic.SteppedOnStand -= OnCubicSteppedOnStand;
        _cubicMovement.CubicLeftPress -= OnCubicLeftPress;
        _blockStack.BlocksEnded -= OnBlockStackBlocksEnded;
        _exitPortal.SuckedIn -= OnExitPortalSuckedIn;
        _menuScreen.StartClicked -= OnMenuStartClicked;
        _dataRestorer.DataRestored -= OnGamaDataRestored;
    }

    public void OnSceneLoaded(LevelLoadConfig levelLoadConfig)
    {
        IsStartWithoutMenu = levelLoadConfig.IsStartWithoutMenu;
    }

    private void OnBlockStackBlocksEnded()
    {
        if (_isCubicSteppedOnStand == false)
        {
            GameEnded?.Invoke(GameResult.LoseWithBlocksEnded);
        }
    }

    private void OnCubicHit(Vector3 contactPoint, float obstacleHeight)
    {
        _cubic.Hit -= OnCubicHit;
        GameEnded?.Invoke(GameResult.LoseWithHit);
    }

    private void OnCubicLeftPress()
    {
        _isCubicLeftPress = true;
    }

    private void OnCubicSteppedOnStand(PressStand pressStand)
    {
        _isCubicSteppedOnStand = true;
    }

    private void OnExitPortalSuckedIn()
    {
        GameEnded?.Invoke(_isCubicLeftPress ? GameResult.Win : GameResult.LoseWithPortalSuckedIn);
    }

    private void OnMenuStartClicked()
    {
        GameStarted?.Invoke();
    }

    private void OnGamaDataRestored(PlayerData playerData)
    {
        if (IsStartWithoutMenu)
        {
            GameStarted?.Invoke();
        }
    }
}