using Agava.YandexGames;
using System;
using UnityEngine;
using UnityEngine.UI;

public class AuthRequestScreen : Screen
{
    [SerializeField] private Button _authButton;

    public event Action PersonalDataPermissed;

    private void OnEnable()
    {
        _authButton.onClick.AddListener(OnAuthButtonClicked);
    }

    private void OnDisable()
    {
        _authButton.onClick.RemoveListener(OnAuthButtonClicked);
    }

    private void OnAuthButtonClicked()
    {
        PlayerAccount.Authorize(OnAuthorized);
    }

    private void OnAuthorized()
    {
        PlayerAccount.RequestPersonalProfileDataPermission(PersonalDataPermissed);
    }
}

