using Agava.YandexGames;
using System;
using System.Collections.Generic;
using IJunior.TypedScenes;
using UnityEngine;

public class GameDataHandler : MonoBehaviour, ISceneLoadHandler<LevelLoadConfig>
{
    [SerializeField] private SkinsRestorer _skinsRestorer;
    [SerializeField] private List<Skin> _skins;
    [SerializeField] private TrainingStageHolder _trainingStageHolder;
    [SerializeField] private SDK _sdk;
    [SerializeField] private bool _deletePlayerPrefs;

    private bool _isUniqueIdGetting;
    private bool _hasAuth;

    public event Action DataRestored;

    public string UniqueID { get; private set; } = "";
    public float Score { get; private set; }
    public int LeaderboardScore { get; private set; }
    public int Level { get; private set; }
    public int TrainingStageNumber { get; private set; } = 1;
    public int TrainingStageAmount { get; private set; }
    public bool IsStartWithoutMenu { get; private set; }
    public bool HasAnonymousKey { get; private set; }
    public IReadOnlyList<Skin> Skins => _skins;

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
    }

    private void Start()
    {
        TrainingStageAmount = _trainingStageHolder.StageAmount;

#if !UNITY_WEBGL || UNITY_EDITOR
        RestoreData();
        DataRestored?.Invoke();
        return;
#endif

        if (YandexGamesSdk.IsInitialized && _isUniqueIdGetting == false)
        {
            TryGetUniqueId();
        }
    }

    private void OnDisable()
    {
        _sdk.Initialized -= OnSDKInitialized;
    }

    public void OnSceneLoaded(LevelLoadConfig levelLoadConfig)
    {
        IsStartWithoutMenu = levelLoadConfig.IsStartWithoutMenu;
        _hasAuth = levelLoadConfig.PlayerHasAuth;
    }

    private void RestoreData()
    {
        const int DefaultValue = 0;

        Score = PlayerPrefs.GetFloat(PlayerPrefsKeys.ScoreKey + UniqueID, DefaultValue);
        LeaderboardScore = PlayerPrefs.GetInt(PlayerPrefsKeys.LeaderboardScoreKey + UniqueID, DefaultValue);
        Level = PlayerPrefs.GetInt(PlayerPrefsKeys.LevelKey + UniqueID, DefaultValue);

        if (Level == DefaultValue)
        {
            TrainingStageNumber = PlayerPrefs.GetInt(PlayerPrefsKeys.TrainingStageKey + UniqueID, TrainingStageNumber);
        }

        _skinsRestorer.Restore(Skins, UniqueID);
    }


    private void ChooseDataForRestoring(bool hasAuth, string defaultUniqueID)
    {
        HasAnonymousKey = PlayerPrefs.HasKey(PlayerPrefsKeys.LeaderboardScoreKey);

        if (hasAuth)
        {
            if (HasAnonymousKey)
            {
                float anonymousLeaderboardScore = PlayerPrefs.GetInt(PlayerPrefsKeys.LeaderboardScoreKey + defaultUniqueID, 0);
                float accountLeaderboardScore = PlayerPrefs.GetInt(PlayerPrefsKeys.LeaderboardScoreKey + UniqueID, 0);

                if (anonymousLeaderboardScore > accountLeaderboardScore)
                {
                    string id = UniqueID;
                    UniqueID = defaultUniqueID;
                    RestoreData();
                    UniqueID = id;
                    DataRestored?.Invoke();
                    return;
                }
            }
        }

        RestoreData();
        DataRestored?.Invoke();
    }

    private void TryGetUniqueId()
    {
        string defaultUniqueID = UniqueID;
        _isUniqueIdGetting = true;

        if (PlayerAccount.IsAuthorized || _hasAuth)
        {
            PlayerAccount.GetProfileData((result) =>
            {
                UniqueID = result.uniqueID;
                ChooseDataForRestoring(_hasAuth, defaultUniqueID);
            });
        }
        else
        {
            ChooseDataForRestoring(_hasAuth, defaultUniqueID);
        }
    }

    private void OnSDKInitialized()
    {
        TryGetUniqueId();
    }
}
