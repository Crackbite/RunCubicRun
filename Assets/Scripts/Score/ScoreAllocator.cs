using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

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
    [SerializeField] private Store _store;

    private float _currentGoodBlockScore;
    private float _goodBlocksInRow;
    private bool _isCubicUnderPress;
    private float _scoreMultiplier = 1f;

    public event Action<Score> ScoreChanged;

    public float ScoreMultiplier
    {
        get => _scoreMultiplier;
        set => _scoreMultiplier = value < 1f ? 1f : value;
    }

    public float TotalScore { get; private set; }

    private void OnEnable()
    {
        _cubic.SteppedOnStand += OnCubicSteppedOnStand;
        _blockStack.BlockAdded += OnBlockAdded;
        _blockStack.BlockRemoved += OnBlockRemoved;
        _store.SkinBought += OnSkinBought;
    }


    private void Start()
    {
        _currentGoodBlockScore = _goodBlockScore;
        ChangeScore(100, ScoreChangeInitiator.Cubic);
    }

    private void OnDisable()
    {
        _cubic.SteppedOnStand -= OnCubicSteppedOnStand;
        _blockStack.BlockAdded -= OnBlockAdded;
        _blockStack.BlockRemoved -= OnBlockRemoved;
        _store.SkinBought -= OnSkinBought;
    }

    public override string ToString()
    {
        return TotalScore.ToString("# ##0", new CultureInfo("ru-RU")).Trim();
    }

    private void ChangeScore(float value, ScoreChangeInitiator initiator)
    {
        const float RoundingFactor = 10.0f;

        TotalScore += Mathf.Round(value * RoundingFactor) / RoundingFactor;
        TotalScore = Mathf.Max(TotalScore, _minScore);
        var score = new Score(TotalScore, value, initiator);

        ScoreChanged?.Invoke(score);
    }

    private void OnBlockAdded(ColorBlock colorBlock)
    {
        _currentGoodBlockScore = _goodBlocksInRow >= _bonusGoodBlockThreshold ? _bonusGoodBlockScore : _goodBlockScore;
        _goodBlocksInRow++;

        float score = _currentGoodBlockScore * ScoreMultiplier;
        ChangeScore(score, ScoreChangeInitiator.Cubic);
    }

    private void OnBlockRemoved(ColorBlock colorBlock)
    {
        _goodBlocksInRow = 0f;

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

    private void OnSkinBought(float skinPrice)
    {
        ChangeScore(-skinPrice, ScoreChangeInitiator.Store);
    }
}