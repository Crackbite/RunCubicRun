using UnityEngine;
using Agava.YandexGames;
using System.Collections;
using System;

public class LeaderboardLoader : MonoBehaviour
{
    [SerializeField] private DataRestorer _dataRestorer;
    [SerializeField] private LeaderboardScreen _leaderboardScreen;

    private WaitForSecondsRealtime _waitForSDKInitializationCheck;

    private const string LeaderboardName = "Leaderboard";
    private const string AnonymousName = "Anonymous";

    public event Action PlayerLogOut;

    public bool HasNotAuth { get; private set; }

    private void OnEnable()
    {
        _dataRestorer.DataRestored += OnGameDataRestored;
    }

    private void OnDisable()
    {
        _dataRestorer.DataRestored -= OnGameDataRestored;
    }

    private void OnGameDataRestored()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        return;
#endif

        if (PlayerAccount.IsAuthorized)
        {
            StartCoroutine(SetScoreToLeaderboard());
        }
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

                int rank = result.entries[i].rank;
                _leaderboardScreen.AddPlayerView(rank, name, result.entries[i].score);
            }
        }, (string _) =>
            {
                HasNotAuth = true;
                PlayerLogOut?.Invoke();
            });
    }

    private IEnumerator SetScoreToLeaderboard()
    {
        while (YandexGamesSdk.IsInitialized == false)
        {
            yield return _waitForSDKInitializationCheck;
        }

        Leaderboard.SetScore(LeaderboardName, _dataRestorer.LeaderboardScore, () =>
        {
            GetPlayerEntries();
        });
    }
}
