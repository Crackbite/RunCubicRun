using System;
using UnityEngine;

public class PlayerPrefsDataSaver : DataSaver
{
    private int _trainingStageAmount;

    public void Init(PlayerData playerData, int trainingStageAmount)
    {
        CurrentPlayerData = playerData;
        _trainingStageAmount = trainingStageAmount;
    }

    protected override void Save(GameResult result)
    {
        int leaderboardScore = (int)(CurrentPlayerData.LeaderboardScore + _scoreAllocator.LevelScore);
        int level = CurrentPlayerData.Level;
        int trainingStage = CurrentPlayerData.TrainingStage;
        float score = result == GameResult.Win ? _scoreAllocator.TotalScore + _scoreAllocator.LevelScore : _scoreAllocator.TotalScore;

        PlayerPrefs.SetInt(PlayerPrefsKeys.LeaderboardScoreKey, leaderboardScore);

        if (IsTrainingStage(level, trainingStage))
        {
            PlayerPrefs.SetInt(PlayerPrefsKeys.TrainingStageKey, ++trainingStage);
        }
        else
        {
            PlayerPrefs.SetInt(PlayerPrefsKeys.LevelKey, ++level);
        }

        SaveScore(score);
    }

    protected override void SaveSettings(bool isOn, SwitchToggle switchToggle)
    {
        if (switchToggle.Type == SettingsType.Music && CurrentPlayerData.IsMusicOn != isOn)
        {
            CurrentPlayerData.ResetSettings(SettingsType.Music, isOn);
            PlayerPrefs.SetInt(PlayerPrefsKeys.MusicToggleKey, Convert.ToInt32(isOn));
        }
        else if (switchToggle.Type == SettingsType.Sound && CurrentPlayerData.IsSoundOn != isOn)
        {
            CurrentPlayerData.ResetSettings(SettingsType.Sound, isOn);
            PlayerPrefs.SetInt(PlayerPrefsKeys.SoundToggleKey, Convert.ToInt32(isOn));
        }
    }

    protected override void SaveSkinState(Skin skin)
    {
        if (CurrentPlayerData != null)
        {
            float score = _scoreAllocator.TotalScore;
            CurrentPlayerData.ResetSkinStates(skin);
            PlayerPrefs.SetInt(skin.ID + PlayerPrefsKeys.ActiveKey, Convert.ToInt32(skin.IsActive));
            PlayerPrefs.SetInt(skin.ID + PlayerPrefsKeys.BoughtKey, Convert.ToInt32(skin.IsBought));
            SaveScore(score);
        }
    }

    private void SaveScore(float score)
    {
        CurrentPlayerData.ResetScore(score);
        PlayerPrefs.SetFloat(PlayerPrefsKeys.ScoreKey, score);
    }


    private bool IsTrainingStage(int level, int trainingStage)
    {
        const int DefaultValue = 0;

        if (level == DefaultValue && trainingStage < _trainingStageAmount)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
