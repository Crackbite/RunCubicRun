using UnityEngine;
using UnityEngine.UI;

public class SettingsScreenHandler : MonoBehaviour
{
    [SerializeField] private Button _activateButton;
    [SerializeField] private SettingsScreen _settingsScreen;
    [SerializeField] private SwitchToggle _musicSwitchToggle;
    [SerializeField] private SwitchToggle _soundSwitchToggle;
    [SerializeField] private DataRestorer _dataRestorer;

    private void Awake()
    {
        _musicSwitchToggle.SetHandlePosition();
        _soundSwitchToggle.SetHandlePosition();
    }

    private void OnEnable()
    {
        _activateButton.onClick.AddListener(OnSettingsScreenActivated);
        _settingsScreen.CloseClicked += OnSettingsCloseClicked;
        _dataRestorer.DataRestored += OnDataRestored;
    }

    private void OnDisable()
    {
        _activateButton.onClick.RemoveListener(OnSettingsScreenActivated);
        _settingsScreen.CloseClicked -= OnSettingsCloseClicked;
        _dataRestorer.DataRestored -= OnDataRestored;
    }

    private void OnSettingsCloseClicked()
    {
        _settingsScreen.Exit();
    }

    private void OnSettingsScreenActivated()
    {
        _settingsScreen.Enter();
    }

    private void OnDataRestored(PlayerData playerData)
    {
        _musicSwitchToggle.ChangeHandlePosition(playerData.IsMusicOn);
        _soundSwitchToggle.ChangeHandlePosition(playerData.IsSoundOn);
    }
}