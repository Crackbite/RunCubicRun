using System;
using UnityEngine;

public class ScoreAllocator : MonoBehaviour
{
    [Range(.5f, 10f)] [SerializeField] private float _scorePerGoodBlock = 1f;
    [Range(-.5f, -10f)] [SerializeField] private float _scorePerBadBlock = -2f;
    [SerializeField] private Cubic _cubic;
    [SerializeField] private BlockStack _blockStack;
    [SerializeField] private PressScoreCalculator _pressScoreCalculator;

    private float _currentScorePerGoodBlock;
    private bool _isCubicUnderPress;
    private float _score;

    public event Action<Score> ScoreChanged;

    private void OnEnable()
    {
        _cubic.SteppedOnStand += OnCubicSteppedOnStand;
        _blockStack.BlockAdded += OnBlockAdded;
        _blockStack.BlockRemoved += OnBlockRemoved;
    }

    private void Start()
    {
        _currentScorePerGoodBlock = _scorePerGoodBlock;
    }

    private void OnDisable()
    {
        _cubic.SteppedOnStand -= OnCubicSteppedOnStand;
        _blockStack.BlockAdded -= OnBlockAdded;
        _blockStack.BlockRemoved -= OnBlockRemoved;
    }

    private void ChangeScore(float value, ScoreChangeInitiator initiator)
    {
        _score = Mathf.Max(_score += value, 0f);
        var score = new Score(_score, value, initiator);

        ScoreChanged?.Invoke(score);
    }

    private void OnBlockAdded(ColorBlock colorBlock)
    {
        ChangeScore(_currentScorePerGoodBlock, ScoreChangeInitiator.Cubic);
    }

    private void OnBlockRemoved(ColorBlock colorBlock)
    {
        float score;
        ScoreChangeInitiator initiator;

        if (_isCubicUnderPress)
        {
            score = _pressScoreCalculator.GetScore(colorBlock);
            initiator = ScoreChangeInitiator.Press;
        }
        else
        {
            score = _scorePerBadBlock;
            initiator = ScoreChangeInitiator.Cubic;
        }

        ChangeScore(score, initiator);
    }

    private void OnCubicSteppedOnStand(PressStand pressStand)
    {
        _isCubicUnderPress = true;
    }
}