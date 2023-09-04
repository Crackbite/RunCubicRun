using System;
using System.Globalization;
using UnityEngine;

public class ScoreAllocator : MonoBehaviour
{
    [SerializeField] private float _minScore;
    [Range(.5f, 10f)][SerializeField] private float _goodBlockScore = 1f;
    [Range(-.5f, -10f)][SerializeField] private float _badBlockScore = -2f;
    [Range(2, 100)][SerializeField] private int _bonusGoodBlockThreshold = 10;
    [Range(1f, 20f)][SerializeField] private float _bonusGoodBlockScore = 1.5f;
    [SerializeField] private Cubic _cubic;
    [SerializeField] private BlockStack _blockStack;
    [SerializeField] private PressScoreCalculator _pressScoreCalculator;
    [SerializeField] private Store _store;
    [SerializeField] private DataRestorer _dataRestorer;
    [SerializeField] private AuthRequestScreen _authRequestScreen;

    private float _currentGoodBlockScore;
    private float _goodBlocksInRow;
    private bool _isCubicUnderPress;
    private float _scoreMultiplier = 1f;
    private float _levelScore;
    private float _totalScore;

    public event Action<Score> ScoreChanged;

    public float ScoreMultiplier
    {
        get => _scoreMultiplier;
        set => _scoreMultiplier = value < 1f ? 1f : value;
    }

    public float TotalScore => _totalScore;
    public float LevelScore => _levelScore;

    private void OnEnable()
    {
        _cubic.SteppedOnStand += OnCubicSteppedOnStand;
        _blockStack.BlockAdded += OnBlockAdded;
        _blockStack.BlockRemoved += OnBlockRemoved;
        _store.SkinBought += OnSkinBought;
        _dataRestorer.DataRestored += OnDataRestored;
        _authRequestScreen.PlayerAuthorized += OnPlayerAuthorized;
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
        _store.SkinBought -= OnSkinBought;
        _dataRestorer.DataRestored -= OnDataRestored;
        _authRequestScreen.PlayerAuthorized -= OnPlayerAuthorized;
    }

    public string ToString(float score)
    {
        return score.ToString("# ##0", new CultureInfo("ru-RU")).Trim();
    }

    private void ChangeScore(ref float scoreType, float value, ScoreChangeInitiator initiator)
    {
        const float RoundingFactor = 10.0f;

        scoreType += Mathf.Round(value * RoundingFactor) / RoundingFactor;
        scoreType = Mathf.Max(scoreType, _minScore);
        var score = new Score(scoreType, value, initiator);

        ScoreChanged?.Invoke(score);
    }

    private void OnDataRestored(PlayerData playerData)
    {
        ChangeScore(ref _totalScore, playerData.Score, ScoreChangeInitiator.DataHandler);
    }

    private void OnBlockAdded(ColorBlock colorBlock)
    {
        _currentGoodBlockScore = _goodBlocksInRow >= _bonusGoodBlockThreshold ? _bonusGoodBlockScore : _goodBlockScore;
        _goodBlocksInRow++;

        float score = _currentGoodBlockScore * ScoreMultiplier;
        ChangeScore(ref _levelScore, score, ScoreChangeInitiator.Cubic);
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

        ChangeScore(ref _levelScore, score, initiator);
    }

    private void OnCubicSteppedOnStand(PressStand pressStand)
    {
        _isCubicUnderPress = true;
    }

    private void OnSkinBought(float skinPrice)
    {
        ChangeScore(ref _totalScore, -skinPrice, ScoreChangeInitiator.Store);
    }

    private void OnPlayerAuthorized()
    {
        const float StartValue = 0;

        _totalScore = _levelScore = StartValue;
    }
}