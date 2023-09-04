using Agava.YandexGames;
using System;
using UnityEngine;

public class DataRestorer : MonoBehaviour
{
    [SerializeField] private SkinsRestorer _skinsRestorer;
    [SerializeField] private TrainingStageHolder _trainingStageHolder;
    [SerializeField] private SDK _sdk;
    [SerializeField] private LevelGenerationStarter _levelGenerationStarter;
    [SerializeField] private bool _deletePlayerPrefs;

    private ChunkGenerator _chunkGenerator;
    private PlayerData _playerData;
    private bool _isDataRestoring;

    public event Action<PlayerData> DataRestored;

    public bool IsJustLoggedIn { get; private set; }
    public int TrainingStageAmount { get; private set; }
    public PlayerData PlayerData => _playerData;

    private void OnValidate()
    {
        if (_deletePlayerPrefs)
        {
            PlayerPrefs.DeleteAll();
        }
    }

    private void OnEnable()
    {
        _sdk.Initialized += OnSDKInitialized;
        _levelGenerationStarter.GeneratorStarted += OnGeneratorStarted;
    }

    private void Start()
    {
        TrainingStageAmount = _trainingStageHolder.StageAmount;

#if !UNITY_WEBGL || UNITY_EDITOR
        RestoreFromPlayerPrefs();
        return;
#endif

        if (YandexGamesSdk.IsInitialized && _isDataRestoring == false)
        {
            Restore();
        }
    }

    private void OnDisable()
    {
        _sdk.Initialized -= OnSDKInitialized;
        _levelGenerationStarter.GeneratorStarted += OnGeneratorStarted;
        _chunkGenerator.ChunksRemoved -= OnChunksRemoved;
    }

    private void Restore()
    {
        _isDataRestoring = true;

        if (PlayerAccount.IsAuthorized || IsJustLoggedIn)
        {
            GetPlayerData(ChooseSaveStorage);
        }
        else
        {
            RestoreFromPlayerPrefs();
        }
    }

    private void RestoreFromCoud(string data)
    {
        _playerData = JsonUtility.FromJson<PlayerData>(data);
        _skinsRestorer.RestoreFromCloud(_playerData);
        DataRestored?.Invoke(_playerData);
    }

    private void RestoreFromPlayerPrefs()
    {
        const int SoundsDefaultValue = 1;
        int defaultValue = 0;

        float score = PlayerPrefs.GetFloat(PlayerPrefsKeys.ScoreKey, defaultValue);
        int leaderboardScore = PlayerPrefs.GetInt(PlayerPrefsKeys.LeaderboardScoreKey, defaultValue);
        int level = PlayerPrefs.GetInt(PlayerPrefsKeys.LevelKey, defaultValue);
        bool isMusicOn = Convert.ToBoolean(PlayerPrefs.GetInt(PlayerPrefsKeys.MusicToggleKey, SoundsDefaultValue));
        bool isSoundOn = Convert.ToBoolean(PlayerPrefs.GetInt(PlayerPrefsKeys.SoundToggleKey, SoundsDefaultValue));
        int trainingStage = defaultValue;
        trainingStage++;

        if (level == defaultValue)
        {
            trainingStage = PlayerPrefs.GetInt(PlayerPrefsKeys.TrainingStageKey, trainingStage);
        }

        _playerData = new PlayerData(score, leaderboardScore, level, trainingStage, isMusicOn, isSoundOn);
        _skinsRestorer.RestoreFromPlayerPrefs(_playerData);
        DataRestored?.Invoke(_playerData);
    }

    private void GetPlayerData(Action<string> callBack)
    {
        PlayerAccount.GetPlayerData((data) =>
        {
            callBack?.Invoke(data);
        });
    }

    private void ChooseSaveStorage(string data)
    {
        const string EmptyData = "{}";

        if (data == EmptyData)
        {
            IsJustLoggedIn = true;
            RestoreFromPlayerPrefs();
        }
        else
        {
            RestoreFromCoud(data);
        }
    }

    private void OnSDKInitialized()
    {
        Restore();
    }

    private void OnChunksRemoved()
    {
        Restore();
    }

    private void OnGeneratorStarted(ChunkGenerator generator)
    {
        _chunkGenerator = generator;
        _chunkGenerator.ChunksRemoved += OnChunksRemoved;
    }
}
