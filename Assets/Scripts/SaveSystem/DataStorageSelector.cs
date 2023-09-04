using Agava.YandexGames;
using System;
using UnityEngine;

public class DataStorageSelector : MonoBehaviour
{
    [SerializeField] private CloudDataSaver _cloudDataSaver;
    [SerializeField] private PlayerPrefsDataSaver _playerPrefsDataSaver;
    [SerializeField] private DataRestorer _dataRestorer;

    public event Action<DataSaver> StorageSelected;

    private void OnEnable()
    {
        _dataRestorer.DataRestored += OnDataRestored;
    }

    private void OnDisable()
    {
        _dataRestorer.DataRestored -= OnDataRestored;
    }

    private void Select(PlayerData playerData)
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        _cloudDataSaver.enabled = false;
        _playerPrefsDataSaver.enabled = true;
        _playerPrefsDataSaver.Init(playerData, _dataRestorer.TrainingStageAmount);
        StorageSelected?.Invoke(_playerPrefsDataSaver);
        return;
#endif

        if (PlayerAccount.IsAuthorized)
        {
            _playerPrefsDataSaver.enabled = false;
            _cloudDataSaver.enabled = true;
            _cloudDataSaver.Init(playerData, _dataRestorer.TrainingStageAmount);
            StorageSelected?.Invoke(_cloudDataSaver);

            if (_dataRestorer.IsJustLoggedIn == false)
            {
                PlayerPrefs.DeleteAll();
            }
        }
        else
        {
            _cloudDataSaver.enabled = false;
            _playerPrefsDataSaver.enabled = true;
            _playerPrefsDataSaver.Init(playerData, _dataRestorer.TrainingStageAmount);
            StorageSelected?.Invoke(_playerPrefsDataSaver);
        }
    }

    private void OnDataRestored(PlayerData playerData)
    {
        Select(playerData);
    }
}
