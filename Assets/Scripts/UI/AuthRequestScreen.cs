using Agava.YandexGames;
using System;
using UnityEngine;
using UnityEngine.UI;

public class AuthRequestScreen : Screen
{
    [SerializeField] private Button _authButton;
    [SerializeField] private Button _closeButton;

    public event Action CloseClicked;
    public event Action AuthClicked;
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
        CloseClicked?.Invoke();
        AuthClicked?.Invoke();

        if (ChunkStorage.Instance != null)
        {
            ChunkStorage.Instance.Restart();
        }

        Authorize();
    }

    private void Authorize()
    {
        PlayerAccount.Authorize(OnAuthorized);
    }

    private void OnAuthorized()
    {
        PlayerAccount.RequestPersonalProfileDataPermission(PlayerAuthorized, (error) => { PlayerAuthorized?.Invoke(); });
    }

    private void OnCloseButtonClicked()
    {
        CloseClicked?.Invoke();
    }
}

