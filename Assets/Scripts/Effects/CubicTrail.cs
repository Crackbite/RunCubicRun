using System;
using UnityEngine;

public class CubicTrail : MonoBehaviour
{
    [SerializeField] private TrailRenderer _trailRenderer;
    [SerializeField] private GameStatusTracker _gameStatusTracker;
    [SerializeField] private PistonPresser _pistonPresser;

    const float LifeTime = 0.5f;

    private void OnEnable()
    {
        _gameStatusTracker.GameEnded += OnGameEnded;
        _pistonPresser.CubicReached += OnCubicReached;
    }

    private void OnDisable()
    {
        _gameStatusTracker.GameEnded -= OnGameEnded;
        _pistonPresser.CubicReached -= OnCubicReached;
    }

    private void OnGameEnded(GameResult result)
    {
        if (result == GameResult.Win)
        {
            return;
        }

        Invoke(nameof(TurnOffTrail), LifeTime);
    }

    private void OnCubicReached(Cubic cubic)
    {
        TurnOffTrail();
    }

    private void TurnOffTrail()
    {
        _trailRenderer.enabled = false;
    }
}
