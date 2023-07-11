using UnityEngine;

public class CubicTrail : MonoBehaviour
{
    [SerializeField] private TrailRenderer _trailRenderer;
    [SerializeField] private GameStatusTracker _gameStatusTracker;
    [SerializeField] private PistonPresser _pistonPresser;
    [SerializeField] private Cubic _cubic;
    [SerializeField] private LevelExitPortal _levelExitPortal;

    private bool _isPressReachedCubic;

    private void OnEnable()
    {
        _gameStatusTracker.GameEnded += OnGameEnded;
        _pistonPresser.CubicReached += OnCubicReached;
        _cubic.SteppedOnStand += OnCubicSteppedOnStand;
        _levelExitPortal.Shaked += OnCubicShaked;
    }

    private void OnDisable()
    {
        _gameStatusTracker.GameEnded -= OnGameEnded;
        _pistonPresser.CubicReached -= OnCubicReached;
        _cubic.SteppedOnStand -= OnCubicSteppedOnStand;
        _levelExitPortal.Shaked -= OnCubicShaked;
    }


    private void OnCubicSteppedOnStand(PressStand stand)
    {
        TurnOffTrail();
    }

    private void OnCubicShaked()
    {
        if (_isPressReachedCubic == false)
        {
            TurnOnTrail();
        }
    }

    private void OnGameEnded(GameResult result)
    {
        if (result == GameResult.Win)
        {
            return;
        }

        TurnOffTrail();
    }

    private void OnCubicReached(Cubic cubic)
    {
        _isPressReachedCubic = true;
    }

    private void TurnOffTrail()
    {
        _trailRenderer.enabled = false;
    }

    private void TurnOnTrail()
    {
        _trailRenderer.enabled = true;
    }
}
