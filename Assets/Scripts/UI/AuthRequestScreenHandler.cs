using Agava.YandexGames;
using UnityEngine;

public class AuthRequestScreenHandler : MonoBehaviour
{
    [SerializeField] private AuthRequestScreen _authRequestScreen;
    [SerializeField] private MenuScreen _menuScreen;

    private void OnEnable()
    {
        _menuScreen.LeaderboardClicked += OnLeaderboardClicked;
        _authRequestScreen.CloseClicked += OnAuthCloseClicked;

    }

    private void OnDisable()
    {
        _menuScreen.LeaderboardClicked -= OnLeaderboardClicked;
        _authRequestScreen.CloseClicked -= OnAuthCloseClicked;
    }

    private void OnAuthCloseClicked()
    {
        _authRequestScreen.Exit();
    }

    private void OnLeaderboardClicked()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        return;
#endif

        if (PlayerAccount.IsAuthorized == false)
        {
            _authRequestScreen.Enter();
        }
    }
}
