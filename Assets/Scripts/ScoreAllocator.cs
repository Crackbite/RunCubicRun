using System;
using UnityEngine;

public class ScoreAllocator : MonoBehaviour
{
    [Range(.5f, 10f)]
    [SerializeField] private float _scorePerGoodBlock = 1f;

    [Range(-.5f, -10f)]
    [SerializeField] private float _scorePerBadBlock = -2f;

    [SerializeField] private BlockStack _blockStack;

    private float _currentScorePerGoodBlock;
    private float _score;

    public event Action<Score> ScoreChanged;

    private void OnEnable()
    {
        _blockStack.BlockAdded += OnBlockAdded;
        _blockStack.BlockRemoved += OnBlockRemoved;
    }

    private void Start()
    {
        _currentScorePerGoodBlock = _scorePerGoodBlock;
    }

    private void OnDisable()
    {
        _blockStack.BlockAdded -= OnBlockAdded;
        _blockStack.BlockRemoved -= OnBlockRemoved;
    }

    private void ChangeScore(float value)
    {
        _score = Mathf.Max(_score += value, 0f);
        var score = new Score(_score, value);

        ScoreChanged?.Invoke(score);
    }

    private void OnBlockAdded(ColorBlock colorBlock)
    {
        ChangeScore(_currentScorePerGoodBlock);
    }

    private void OnBlockRemoved(ColorBlock colorBlock)
    {
        ChangeScore(_scorePerBadBlock);
    }
}