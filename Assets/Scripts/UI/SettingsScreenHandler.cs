using UnityEngine;
using UnityEngine.UI;

public class SettingsScreenHandler : MonoBehaviour
{
    [SerializeField] private Button _activateButton;
    [SerializeField] private SettingsScreen _settingsScreen;

    private void OnEnable()
    {
        _activateButton.onClick.AddListener(OnSettingsScreenActivated);
        _settingsScreen.CloseClicked += OnSettingsCloseClicked;
    }

    private void OnDisable()
    {
        _activateButton.onClick.RemoveListener(OnSettingsScreenActivated);
        _settingsScreen.CloseClicked -= OnSettingsCloseClicked;
    }

    private void OnSettingsCloseClicked()
    {
        _settingsScreen.Exit();
    }

    private void OnSettingsScreenActivated()
    {
        _settingsScreen.Enter();
    }
}