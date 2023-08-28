using UnityEngine;
using Agava.YandexGames;
using System.Collections;

public class LeaderboardLoader : MonoBehaviour
{
    [SerializeField] private GameDataHandler _gameDataHandler;
    [SerializeField] private LeaderboardScreen _leaderboardScreen;
    [SerializeField] private AuthRequestScreen _authRequestScreen;

    private WaitForSecondsRealtime _waitForSDKInitializationCheck;

    private const string LeaderboardName = "Leaderboard";
    private const string AnonymousName = "Anonymous";

    private void OnEnable()
    {
        _authRequestScreen.PersonalDataPermissed += OnPersonalDataPermissed;
        _gameDataHandler.DataRestored += OnGameDataRestored;
    }

    private void OnDisable()
    {
        _authRequestScreen.PersonalDataPermissed -= OnPersonalDataPermissed;
        _gameDataHandler.DataRestored -= OnGameDataRestored;
    }

    private void OnPersonalDataPermissed()
    {
        GetPlayerEntries();
    }

    private void OnGameDataRestored()
    {
        StartCoroutine(SetScoreToLeaderboard());
    }

    private void GetPlayerEntries()
    {
        Leaderboard.GetEntries(LeaderboardName, (result) =>
        {
            for (int i = 0; i < result.entries.Length; i++)
            {
                string name = result.entries[i].player.publicName;

                if (string.IsNullOrEmpty(name))
                {
                    name = AnonymousName;
                }

                _leaderboardScreen.AddPlayerView(i + 1, name, result.entries[i].score);
            }
        });
    }

    private IEnumerator SetScoreToLeaderboard()
    {
        while (YandexGamesSdk.IsInitialized == false)
        {
            yield return _waitForSDKInitializationCheck;
        }

        Leaderboard.SetScore(LeaderboardName, _gameDataHandler.LeaderboardScore, () =>
        {
            GetPlayerEntries();
        });
    }
}
