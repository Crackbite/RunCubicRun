using System;
using UnityEngine;

public class GameStatusTracker : MonoBehaviour
{
    [SerializeField] private Cubic _cubic;
    [SerializeField] private CubicMovement _cubicMovement;
    [SerializeField] private BlockStack _blockStack;
    [SerializeField] private LevelExitPortal _exitPortal;
    [SerializeField] private MenuScreen _menuScreen;

    private bool _isCubicLeftPress;
    private bool _isCubicSteppedOnStand;

    public event Action<GameResult> GameEnded;
    public event Action GameStarted;

    private void OnEnable()
    {
        _cubic.Hit += OnCubicHit;
        _cubic.SteppedOnStand += OnCubicSteppedOnStand;
        _cubicMovement.CubicLeftPress += OnCubicLeftPress;
        _blockStack.BlocksEnded += OnBlockStackBlocksEnded;
        _exitPortal.SuckedIn += OnExitPortalSuckedIn;
        _menuScreen.StartClicked += OnMenuStartClicked;
    }

    private void OnDisable()
    {
        _cubic.Hit -= OnCubicHit;
        _cubic.SteppedOnStand -= OnCubicSteppedOnStand;
        _cubicMovement.CubicLeftPress -= OnCubicLeftPress;
        _blockStack.BlocksEnded -= OnBlockStackBlocksEnded;
        _exitPortal.SuckedIn -= OnExitPortalSuckedIn;
        _menuScreen.StartClicked -= OnMenuStartClicked;
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
}