using UnityEngine;

public class Multiplier : Bonus
{
    [SerializeField] private float _scoreMultiplier = 2f;
    [SerializeField] private ScoreAllocator _scoreAllocator;

    private float _initialScoreMultiplier;

    public override void Apply()
    {
        _initialScoreMultiplier = _scoreAllocator.ScoreMultiplier;
        _scoreAllocator.ScoreMultiplier = _scoreMultiplier;
    }

    public override void Cancel()
    {
        _scoreAllocator.ScoreMultiplier = _initialScoreMultiplier;
    }
}