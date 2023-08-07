using System;
using System.Collections.Generic;
using UnityEngine;

public class GameDataHandler : MonoBehaviour
{
    [SerializeField] private SkinsRestorer _skinsRestorer;
    [SerializeField] private List<Skin> _skins;
    [SerializeField] private TrainingStageHolder _trainingStageHolder;
    [SerializeField] private SuccessScreen _successScreen;
    [SerializeField] private FailScreen _failScreen;
    [SerializeField] private bool _deletePlayerPrefs;

    public event Action DataRestored;

    public float Score { get; private set; }
    public int Level { get; private set; }
    public int TrainingStageNumber { get; private set; } = 1;
    public int TrainingStageAmount { get; private set; }
    public bool IsLevelRestarting { get; private set; }
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
        _successScreen.NextLevelLoading += OnNextLevelLoaging;
        _failScreen.LevelRestarting += OnLevelrestarting;
    }

    private void Start()
    {
        const int DefaultValue = 0;

        IsLevelRestarting = Convert.ToBoolean(PlayerPrefs.GetInt(PlayerPrafsKeys.RestartKey, DefaultValue));
        TrainingStageAmount = _trainingStageHolder.StageAmount;
        Score = PlayerPrefs.GetFloat(PlayerPrafsKeys.ScoreKey, DefaultValue);
        Level = PlayerPrefs.GetInt(PlayerPrafsKeys.LevelKey, DefaultValue);

        if (Level == DefaultValue)
        {
            TrainingStageNumber = PlayerPrefs.GetInt(PlayerPrafsKeys.TrainingStageKey, TrainingStageNumber);
        }

        _skinsRestorer.Restore(Skins);
        DataRestored?.Invoke();
        IsLevelRestarting = false;
    }

    private void OnDisable()
    {
        _successScreen.NextLevelLoading -= OnNextLevelLoaging;
        _failScreen.LevelRestarting -= OnLevelrestarting;
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
