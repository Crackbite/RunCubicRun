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

    private const string EmptyData = "{}";
    private const string PlayerDataKey = nameof(PlayerDataKey);

    private ChunkGenerator _chunkGenerator;
    private PlayerData _playerData;
    private bool _isDataRestoring;

    public event Action<PlayerData> DataRestored;

    public bool IsJustLoggedIn { get; private set; }
    public int TrainingStageAmount { get; private set; }
    public int CurrentLevel => _playerData.Level;
    public int CurrentTrainingStage => _playerData.TrainingStage;

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
        GetPlayerPrefsData(CompleteRestoring);
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

        if (PlayerAccount.IsAuthorized)
        {
            GetCloudData(CheckIsJustLoggedIn);
        }
        else
        {
            GetPlayerPrefsData(CompleteRestoring);
        }
    }

    private void GetCloudData(Action<string> callBack)
    {
        PlayerAccount.GetPlayerData((data) =>
        {
            callBack?.Invoke(data);
        });
    }

    private void GetPlayerPrefsData(Action<string> callBack)
    {
        string data = PlayerPrefs.GetString(PlayerDataKey, EmptyData);
        callBack?.Invoke(data);
    }

    private void CompleteRestoring(string data)
    {
        _playerData = JsonUtility.FromJson<PlayerData>(data);

        if (data == EmptyData)
        {
            _playerData.ResetSettings(SettingsType.Sound, true);
            _playerData.ResetSettings(SettingsType.Music, true);
        }

        _skinsRestorer.RestoreSkinStateInfos(_playerData);
        DataRestored?.Invoke(_playerData);
    }

    private void CheckIsJustLoggedIn(string data)
    {
        if (data == EmptyData)
        {
            IsJustLoggedIn = true;
            GetPlayerPrefsData(CompleteRestoring);
            return;
        }

        CompleteRestoring(data);
    }

    private void OnSDKInitialized()
    {
        Restore();
    }

    private void OnChunksRemoved()
    {
        _chunkGenerator.ChunksRemoved -= OnChunksRemoved;
        Restore();
    }

    private void OnGeneratorStarted(ChunkGenerator generator)
    {
        _chunkGenerator = generator;
        _chunkGenerator.ChunksRemoved += OnChunksRemoved;
    }
}
