using System.Collections.Generic;
using UnityEngine;

public abstract class DataSaver : MonoBehaviour
{
    [SerializeField] protected ScoreAllocator _scoreAllocator;
    [SerializeField] private GameStatusTracker _gameStatusTracker;
    [SerializeField] private SwitchToggle _musicSwitchToggle;
    [SerializeField] private SwitchToggle _soundSwitchToggle;

    private List<Skin> _skins = new List<Skin>();

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

    public void SubscribeToSkinChanges(Skin skin)
    {
        _skins.Add(skin);
        skin.ActivityChanged += OnSkinActivityChanged;
    }

    protected abstract void Save(GameResult result);
    protected abstract void SaveSettings(bool isOn, SwitchToggle switchToggle);
    protected abstract void SaveSkinState(Skin skin);

    protected void SetSkinStatesToPlayerData(PlayerData playerData)
    {
        playerData.SetSkinsStateInfos(_skins);
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