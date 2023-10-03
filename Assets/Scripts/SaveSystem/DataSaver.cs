using GameAnalyticsSDK;
using System.Collections.Generic;
using UnityEngine;

public abstract class DataSaver : MonoBehaviour
{
    [SerializeField] protected ScoreAllocator _scoreAllocator;
    [SerializeField] private GameStatusTracker _gameStatusTracker;
    [SerializeField] private SwitchToggle _musicSwitchToggle;
    [SerializeField] private SwitchToggle _soundSwitchToggle;
    [SerializeField] private LeaderboardLoader _leaderBoardLoader;

    private List<Skin> _skins = new List<Skin>();
    private int _trainingStageAmount;

    protected PlayerData CurrentPlayerData;

    private void OnEnable()
    {
        _gameStatusTracker.GameEnded += OnGameEnded;
        _musicSwitchToggle.ToggleChanged += OnSwitchToggleChanged;
        _soundSwitchToggle.ToggleChanged += OnSwitchToggleChanged;
    }

    private void OnDisable()
    {
        _gameStatusTracker.GameEnded -= OnGameEnded;
        _musicSwitchToggle.ToggleChanged -= OnSwitchToggleChanged;
        _soundSwitchToggle.ToggleChanged -= OnSwitchToggleChanged;

        foreach (Skin skin in _skins)
        {
            skin.ActivityChanged -= OnSkinActivityChanged;
        }
    }

    public void Init(PlayerData playerData, int trainingStageAmount)
    {
        CurrentPlayerData = playerData;
        _trainingStageAmount = trainingStageAmount;
    }

    public void SubscribeToSkinChanges(Skin skin)
    {
        _skins.Add(skin);
        skin.ActivityChanged += OnSkinActivityChanged;
    }

    protected abstract void SaveToStorage(string data);

    protected void Save(GameResult result)
    {
        int leaderboardScore = (int)(CurrentPlayerData.LeaderboardScore + _scoreAllocator.LevelScore);
        int level = CurrentPlayerData.Level;
        int trainingStage = CurrentPlayerData.TrainingStage;
        float score = result == GameResult.Win ? _scoreAllocator.TotalScore + _scoreAllocator.LevelScore : _scoreAllocator.TotalScore;
        bool isMusicOn = CurrentPlayerData.IsMusicOn;
        bool isSoundOn = CurrentPlayerData.IsSoundOn;


        if (IsNextTrainingStage(level, trainingStage))
        {
            trainingStage++;
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, $"The player progressed to stage {trainingStage} of training");
        }
        else
        {
            level++;
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, $"The player progressed to level  {level}");
        }

        PlayerData playerData = new PlayerData(score, leaderboardScore, level, trainingStage, isMusicOn, isSoundOn);

        SetSkinStatesToPlayerData(playerData);
        _leaderBoardLoader.SetScore(leaderboardScore);
        CurrentPlayerData = playerData;
        SaveToStorage(SerializePlayerDataToString());
    }

    protected void SaveChunks()
    {
        const int FirstLevel = 1;

        if (CurrentPlayerData.Level >= FirstLevel)
        {
            CurrentPlayerData.SetChunksData(ChunkStorage.Instance.Chunks);
            SaveToStorage(SerializePlayerDataToString());
        }
    }

    private void SaveSettings(bool isOn, SwitchToggle switchToggle)
    {
        if (switchToggle.Type == SettingsType.Music && CurrentPlayerData.IsMusicOn != isOn)
        {
            CurrentPlayerData.ResetSettings(SettingsType.Music, isOn);
            SaveToStorage(SerializePlayerDataToString());
        }
        else if (switchToggle.Type == SettingsType.Sound && CurrentPlayerData.IsSoundOn != isOn)
        {
            CurrentPlayerData.ResetSettings(SettingsType.Sound, isOn);
            SaveToStorage(SerializePlayerDataToString());
        }
    }

    private void SaveSkinState(Skin skin)
    {
        if (CurrentPlayerData != null)
        {
            float score = _scoreAllocator.TotalScore;
            CurrentPlayerData.ResetSkinStates(skin);
            CurrentPlayerData.ResetScore(score);
            SaveToStorage(SerializePlayerDataToString());
        }
    }

    private string SerializePlayerDataToString()
    {
        return JsonUtility.ToJson(CurrentPlayerData);
    }

    private void SetSkinStatesToPlayerData(PlayerData playerData)
    {
        playerData.SetSkinsStateInfos(_skins);
    }

    private bool IsNextTrainingStage(int level, int trainingStage)
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

    private void OnGameEnded(GameResult result)
    {
        if (result == GameResult.Win)
        {
            if (ChunkStorage.Instance != null)
            {
                ChunkStorage.Instance.Restart();
            }

            Save(result);
        }
        else
        {
            SaveChunks();
        }
    }

    private void OnSwitchToggleChanged(bool isOn, SwitchToggle switchToggle)
    {
        SaveSettings(isOn, switchToggle);
    }

    private void OnSkinActivityChanged(Skin skin)
    {
        SaveSkinState(skin);
    }
}