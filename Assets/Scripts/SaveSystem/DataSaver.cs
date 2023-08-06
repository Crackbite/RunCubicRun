using System;
using System.Collections.Generic;
using UnityEngine;

public class DataSaver : MonoBehaviour
{
    [SerializeField] GameDataHandler _dataHolder;
    [SerializeField] private ScoreAllocator _scoreAllocator;
    [SerializeField] private GameStatusTracker _gameStatusTracker;

    private List<Skin> _skins = new List<Skin>();
    private bool _isSkinBought;

    private void OnEnable()
    {
        _gameStatusTracker.GameEnded += OnGameEnded;
    }

    private void OnDisable()
    {
        _gameStatusTracker.GameEnded -= OnGameEnded;

        foreach (Skin skin in _skins)
        {
            skin.ActivityChanged -= OnSkinActivityChanged;
            skin.Bought -= OnSkinBought;
        }

        SaveIsLevelRestarting();
    }

    public void SubscribeToSkinChanges(Skin skin)
    {
        _skins.Add(skin);
        skin.ActivityChanged += OnSkinActivityChanged;
        skin.Bought += OnSkinBought;
    }

    private void SaveIsLevelRestarting()
    {
        PlayerPrefs.SetInt(PlayerPrafsKeys.RestartKey, Convert.ToInt32(_dataHolder.IsLevelRestarting));
    }

    private void OnSkinBought(Skin skin)
    {
        PlayerPrefs.SetInt(skin.ID + PlayerPrafsKeys.BoughtKey, Convert.ToInt32(skin.IsBought));
        _isSkinBought = true;
    }

    private void OnSkinActivityChanged(Skin skin)
    {
        PlayerPrefs.SetInt(skin.ID + PlayerPrafsKeys.ActiveKey, Convert.ToInt32(skin.IsActive));
    }

    private void OnGameEnded(GameResult result)
    {
        const int DefaultValue = 0;

        if (result == GameResult.Win)
        {
            float score = _scoreAllocator.TotalScore + _scoreAllocator.LevelScore;
            PlayerPrefs.SetFloat(PlayerPrafsKeys.ScoreKey, score);

            if (_dataHolder.Level == DefaultValue && _dataHolder.TrainingStageNumber < _dataHolder.TrainingStageAmount)
            {
                PlayerPrefs.SetInt(PlayerPrafsKeys.TrainingStageKey, _dataHolder.TrainingStageNumber + 1);
            }
            else
            {
                PlayerPrefs.SetInt(PlayerPrafsKeys.LevelKey, _dataHolder.Level + 1);
            }
        }
        else if (_isSkinBought)
        {
            PlayerPrefs.SetFloat(PlayerPrafsKeys.ScoreKey, _scoreAllocator.TotalScore);
        }
    }
}
