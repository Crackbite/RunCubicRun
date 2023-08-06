using System;
using System.Collections.Generic;
using UnityEngine;

public class GameDataHandler : MonoBehaviour
{
    [SerializeField] private ScoreAllocator _scoreAllocator;
    [SerializeField] private GameStatusTracker _gameStatusTracker;
    [SerializeField] private List<Skin> _skins;
    [SerializeField] private bool _deletePlayerPrefs;
    [SerializeField] private TrainingStageHolder _trainingStageHolder;
    [SerializeField] private SuccessScreen _successScreen;
    [SerializeField] private FailScreen _failScreen;

    private float _score;
    private int _level;
    private int _trainingStage = 1;
    private int _trainingStageAmount;
    private bool _isActiveSkinChoosed;
    private bool _isSkinBought;

    public event Action DataRestored;

    public float Score => _score;
    public int Level => _level;
    public int TrainingStageNumber => _trainingStage;
    public int TrainingStageAmount => _trainingStageAmount;
    public IReadOnlyList<Skin> Skins => _skins;
    public bool IsLevelRestarting { get; private set; }

    private void OnValidate()
    {
        if (_deletePlayerPrefs)
        {
            PlayerPrefs.DeleteAll();
        }
    }

    private void OnEnable()
    {
        _gameStatusTracker.GameEnded += OnGameEnded;
        _successScreen.NextLevelLoading += OnNextLevelLoaging;
        _failScreen.LevelRestarting += OnLevelrestarting;
    }

    private void Start()
    {
        const int DefaultValue = 0;

        IsLevelRestarting = Convert.ToBoolean(PlayerPrefs.GetInt(PlayerPrafsKeys.RestartKey, DefaultValue));
        _trainingStageAmount = _trainingStageHolder.StageAmount;
        _score = PlayerPrefs.GetFloat(PlayerPrafsKeys.ScoreKey, DefaultValue);
        _level = PlayerPrefs.GetInt(PlayerPrafsKeys.LevelKey, DefaultValue);

        if (_level == DefaultValue)
        {
            _trainingStage = PlayerPrefs.GetInt(PlayerPrafsKeys.TrainingStageKey, _trainingStage);
        }

        foreach (Skin skin in _skins)
        {
            skin.ActivityChanged += OnSkinActivityChanged;
            skin.Bought += OnSkinBought;

            if (RestoreBooleanData(PlayerPrafsKeys.BoughtKey, skin.ID))
            {
                skin.Buy();
            }

            if (RestoreBooleanData(PlayerPrafsKeys.ActiveKey, skin.ID) && _isActiveSkinChoosed == false)
            {
                skin.TurnOnActivity();
                _isActiveSkinChoosed = true;
            }
            else
            {
                skin.TurnOffActivity();
            }
        }

        if (_isActiveSkinChoosed == false)
        {
            _skins[0].Buy();
            _skins[0].TurnOnActivity();
        }

        DataRestored?.Invoke();
        IsLevelRestarting = false;
    }

    private void OnDisable()
    {
        _gameStatusTracker.GameEnded -= OnGameEnded;
        _successScreen.NextLevelLoading -= OnNextLevelLoaging;
        _failScreen.LevelRestarting -= OnLevelrestarting;

        foreach (Skin skin in _skins)
        {
            skin.ActivityChanged -= OnSkinActivityChanged;
            skin.Bought -= OnSkinBought;
        }

        SaveIsLevelRestarting();
    }

    private bool RestoreBooleanData(string key, string skinID)
    {
        const int FalseIndex = 0;

        return Convert.ToBoolean(PlayerPrefs.GetInt(skinID + key, FalseIndex));
    }

    private void SaveIsLevelRestarting()
    {
        PlayerPrefs.SetInt(PlayerPrafsKeys.RestartKey, Convert.ToInt32(IsLevelRestarting));
    }

    private void OnGameEnded(GameResult result)
    {
        const int DefaultValue = 0;

        if (result == GameResult.Win)
        {
            _score = _scoreAllocator.TotalScore + _scoreAllocator.LevelScore;
            PlayerPrefs.SetFloat(PlayerPrafsKeys.ScoreKey, _score);

            if (_level == DefaultValue && _trainingStage < _trainingStageAmount)
            {
                PlayerPrefs.SetInt(PlayerPrafsKeys.TrainingStageKey, _trainingStage + 1);
            }
            else
            {
                PlayerPrefs.SetInt(PlayerPrafsKeys.LevelKey, _level + 1);
            }
        }
        else if (_isSkinBought)
        {
            PlayerPrefs.SetFloat(PlayerPrafsKeys.ScoreKey, _scoreAllocator.TotalScore);
        }
    }

    private void OnSkinBought(Skin skin)
    {
        PlayerPrefs.SetInt(skin.ID + PlayerPrafsKeys.BoughtKey, Convert.ToInt32(true));
        _isSkinBought = true;
    }

    private void OnSkinActivityChanged(Skin skin)
    {
        PlayerPrefs.SetInt(skin.ID + PlayerPrafsKeys.ActiveKey, Convert.ToInt32(skin.IsActive));
    }

    private void OnNextLevelLoaging()
    {
        IsLevelRestarting = true;
    }

    private void OnLevelrestarting()
    {
        IsLevelRestarting = true;
    }
}
