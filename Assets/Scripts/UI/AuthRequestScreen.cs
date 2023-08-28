using Agava.YandexGames;
using System;
using UnityEngine;
using UnityEngine.UI;

public class AuthRequestScreen : Screen
{
    [SerializeField] private Button _authButton;
    [SerializeField] private Button _closeButton;

    public event Action CloseClicked;
    public event Action PlayerAuthorized;

    private void OnEnable()
    {
        _authButton.onClick.AddListener(OnAuthButtonClicked);
        _closeButton.onClick.AddListener(OnCloseButtonClicked);
    }

    private void OnDisable()
    {
        _authButton.onClick.RemoveListener(OnAuthButtonClicked);
        _closeButton.onClick.RemoveListener(OnCloseButtonClicked);
    }

    private void OnAuthButtonClicked()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        CloseClicked?.Invoke();
        return;
#endif

        CloseClicked?.Invoke();
        PlayerAccount.Authorize(OnAuthorized);
    }

    private void OnAuthorized()
    {
        PlayerAccount.RequestPersonalProfileDataPermission(PlayerAuthorized);
    }

    private void OnCloseButtonClicked()
    {
        CloseClicked?.Invoke();
    }
}

