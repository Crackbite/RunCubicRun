using Agava.YandexGames;
using UnityEngine;

public class CloudDataSaver : DataSaver
{
    public void Init(PlayerData playerData)
    {
        CurrentPlayerData = playerData;
    }

    protected override void Save(GameResult result)
    {
        int leaderboardScore = (int)(CurrentPlayerData.LeaderboardScore + _scoreAllocator.LevelScore);
        int level = CurrentPlayerData.Level;
        int trainingStage = CurrentPlayerData.TrainingStage;
        float score = result == GameResult.Win ? _scoreAllocator.TotalScore + _scoreAllocator.LevelScore : _scoreAllocator.TotalScore;
        bool isMusicOn = CurrentPlayerData.IsMusicOn;
        bool isSoundOn = CurrentPlayerData.IsSoundOn;
        PlayerData playerData = new PlayerData(score, leaderboardScore, level, trainingStage, isMusicOn, isSoundOn);
        SetSkinStatesToPlayerData(playerData);
        CurrentPlayerData = playerData;
        SaveDataToCloud(CurrentPlayerData);
    }

    protected override void SaveSettings(bool isOn, SwitchToggle switchToggle)
    {
        if (switchToggle.Type == SettingsType.Music && CurrentPlayerData.IsMusicOn != isOn)
        {
            CurrentPlayerData.ResetSettings(SettingsType.Music, isOn);
            SaveDataToCloud(CurrentPlayerData);

        }
        else if (switchToggle.Type == SettingsType.Sound && CurrentPlayerData.IsSoundOn != isOn)
        {
            CurrentPlayerData.ResetSettings(SettingsType.Sound, isOn);
            SaveDataToCloud(CurrentPlayerData);
        }
    }

    protected override void SaveSkinState(Skin skin)
    {
        if (CurrentPlayerData != null)
        {
            float score = _scoreAllocator.TotalScore;
            CurrentPlayerData.ResetSkinStates(skin);
            CurrentPlayerData.ResetScore(score);
            SaveDataToCloud(CurrentPlayerData);
        }
    }

    private void SaveDataToCloud(PlayerData playerData)
    {
        string playerDataJson = JsonUtility.ToJson(playerData);
        PlayerAccount.SetPlayerData(playerDataJson);
    }
}
