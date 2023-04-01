using System;
using UnityEngine;

public class ScoreAllocator : MonoBehaviour
{
    [SerializeField] private float _minScore;
    [Range(.5f, 10f)] [SerializeField] private float _goodBlockScore = 1f;
    [Range(-.5f, -10f)] [SerializeField] private float _badBlockScore = -2f;
    [Range(2, 100)] [SerializeField] private int _bonusGoodBlockThreshold = 10;
    [Range(1f, 20f)] [SerializeField] private float _bonusGoodBlockScore = 1.5f;
    [SerializeField] private Cubic _cubic;
    [SerializeField] private BlockStack _blockStack;
    [SerializeField] private PressScoreCalculator _pressScoreCalculator;

    private float _currentGoodBlockScore;
    private float _goodBlocksInRow;
    private bool _isCubicUnderPress;
    private float _totalScore;

    public event Action<Score> ScoreChanged;

    private void OnEnable()
    {
        _cubic.SteppedOnStand += OnCubicSteppedOnStand;
        _blockStack.BlockAdded += OnBlockAdded;
        _blockStack.BlockRemoved += OnBlockRemoved;
    }

    private void Start()
    {
        _currentGoodBlockScore = _goodBlockScore;
    }

    private void OnDisable()
    {
        _cubic.SteppedOnStand -= OnCubicSteppedOnStand;
        _blockStack.BlockAdded -= OnBlockAdded;
        _blockStack.BlockRemoved -= OnBlockRemoved;
    }

    private void ChangeScore(float value, ScoreChangeInitiator initiator)
    {
        const float RoundingFactor = 10.0f;

        _totalScore += Mathf.Round(value * RoundingFactor) / RoundingFactor;
        _totalScore = Mathf.Max(_totalScore, _minScore);
        var score = new Score(_totalScore, value, initiator);

        ScoreChanged?.Invoke(score);
    }

    private void OnBlockAdded(ColorBlock colorBlock)
    {
        _currentGoodBlockScore = _goodBlocksInRow >= _bonusGoodBlockThreshold ? _bonusGoodBlockScore : _goodBlockScore;
        _goodBlocksInRow++;

        ChangeScore(_currentGoodBlockScore, ScoreChangeInitiator.Cubic);
    }

    private void OnBlockRemoved(ColorBlock colorBlock)
    {
        _goodBlocksInRow = 0;

        float score;
        ScoreChangeInitiator initiator;

        if (_isCubicUnderPress)
        {
            score = _pressScoreCalculator.GetScore(colorBlock);
            initiator = ScoreChangeInitiator.Press;
        }
        else
        {
            score = _badBlockScore;
            initiator = ScoreChangeInitiator.Cubic;
        }

        ChangeScore(score, initiator);
    }

    private void OnCubicSteppedOnStand(PressStand pressStand)
    {
        _isCubicUnderPress = true;
    }
}