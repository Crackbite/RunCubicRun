using System;
using System.Collections.Generic;
using UnityEngine;

public class DataSaver : MonoBehaviour
{
    [SerializeField] GameDataHandler _dataHolder;
    [SerializeField] private ScoreAllocator _scoreAllocator;
    [SerializeField] private GameStatusTracker _gameStatusTracker;
    [SerializeField] private SwitchToggle _musicSwitchToggle;
    [SerializeField] private SwitchToggle _soundSwitchToggle;

    private List<Skin> _skins = new List<Skin>();
    private Skin _boughtSkin;
    private string _uniqueID;

    private void OnEnable()
    {
        _gameStatusTracker.GameEnded += OnGameEnded;
        _dataHolder.DataRestored += OnDataRestored;
        _musicSwitchToggle.ToggleChanged += OnSwitchToggleChanged;
        _soundSwitchToggle.ToggleChanged += OnSwitchToggleChanged;
    }

    private void OnDisable()
    {
        _gameStatusTracker.GameEnded -= OnGameEnded;
        _dataHolder.DataRestored -= OnDataRestored;
        _musicSwitchToggle.ToggleChanged -= OnSwitchToggleChanged;
        _soundSwitchToggle.ToggleChanged -= OnSwitchToggleChanged;

        foreach (Skin skin in _skins)
        {
            skin.ActivityChanged -= OnSkinActivityChanged;
            skin.Bought -= OnSkinBought;
        }
    }

    public void SubscribeToSkinChanges(Skin skin)
    {
        _skins.Add(skin);
        skin.ActivityChanged += OnSkinActivityChanged;
        skin.Bought += OnSkinBought;
    }

    private void SaveLevelData(GameResult result)
    {
        const int DefaultValue = 0;

        if (result == GameResult.Win)
        {
            int leaderboardScore = (int)(_dataHolder.LeaderboardScore + _scoreAllocator.LevelScore);
            PlayerPrefs.SetInt(PlayerPrefsKeys.LeaderboardScoreKey + _uniqueID, leaderboardScore);

            if (_dataHolder.Level == DefaultValue && _dataHolder.TrainingStageNumber < _dataHolder.TrainingStageAmount)
            {
                PlayerPrefs.SetInt(PlayerPrefsKeys.TrainingStageKey + _uniqueID, _dataHolder.TrainingStageNumber + 1);
            }
            else
            {
                PlayerPrefs.SetInt(PlayerPrefsKeys.LevelKey + _uniqueID, _dataHolder.Level + 1);
            }
        }

        float score = result == GameResult.Win ? _scoreAllocator.TotalScore + _scoreAllocator.LevelScore : _scoreAllocator.TotalScore;
        PlayerPrefs.SetFloat(PlayerPrefsKeys.ScoreKey + _uniqueID, score);

        if (_boughtSkin != null)
        {
            PlayerPrefs.SetInt(_boughtSkin.ID + PlayerPrefsKeys.BoughtKey + _uniqueID, Convert.ToInt32(_boughtSkin.IsBought));
        }
    }

    private void ClearAnonymousData()
    {
        foreach (string key in PlayerPrefsKeys.Keys)
        {
            if (key == PlayerPrefsKeys.BoughtKey || key == PlayerPrefsKeys.ActiveKey)
            {
                foreach (Skin skin in _dataHolder.Skins)
                {
                    PlayerPrefs.DeleteKey(skin.ID + key);
                }
            }
            else
            {
                PlayerPrefs.DeleteKey(key);
            }
        }
    }

    private void OnDataRestored()
    {
        const string anonymousID = "";

        _uniqueID = _dataHolder.UniqueID;

        if (_dataHolder.HasAnonymousKey && _dataHolder.UniqueID != anonymousID)
        {
            ClearAnonymousData();
        }
    }

    private void OnSkinBought(Skin skin)
    {
        _boughtSkin = skin;
    }

    private void OnSkinActivityChanged(Skin skin)
    {
        PlayerPrefs.SetInt(skin.ID + PlayerPrefsKeys.ActiveKey + _uniqueID, Convert.ToInt32(skin.IsActive));
    }

    private void OnGameEnded(GameResult result)
    {
        SaveLevelData(result);
    }

    private void OnSwitchToggleChanged(bool isOn, SwitchToggle switchToggle)
    {
        if (switchToggle.Type == SwitchToggleType.Music)
        {
            PlayerPrefs.SetInt(PlayerPrefsKeys.MusicToggleKey + _uniqueID, Convert.ToInt32(isOn));
        }
        else
        {
            PlayerPrefs.SetInt(PlayerPrefsKeys.SoundToggleKey + _uniqueID, Convert.ToInt32(isOn));
        }
    }
}
