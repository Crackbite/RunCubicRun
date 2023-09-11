using UnityEngine;
using Agava.YandexGames;
using System.Collections;
using System;

public class LeaderboardLoader : MonoBehaviour
{
    [SerializeField] private DataRestorer _dataRestorer;
    [SerializeField] private LeaderboardScreen _leaderboardScreen;

    private WaitForSecondsRealtime _waitForSDKInitializationCheck;
    private bool _hasResult;

    private const string LeaderboardName = "Leaderboard";
    private const string AnonymousName = "Anonymous";

    public event Action PlayerLogOut;

    private void OnEnable()
    {
        _dataRestorer.DataRestored += OnGameDataRestored;
    }

    private void OnDisable()
    {
        _dataRestorer.DataRestored -= OnGameDataRestored;
    }

    public void SetScore(PlayerData playerData)
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        return;
#endif

        Leaderboard.SetScore(LeaderboardName, playerData.LeaderboardScore, () =>
        {
            GetPlayerEntries(playerData);
        });
    }

    private void OnGameDataRestored(PlayerData playerData)
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        return;
#endif

        if (PlayerAccount.IsAuthorized)
        {
            StartCoroutine(WaitForSDKInitialization(playerData));
        }
    }

    private void GetPlayerEntries(PlayerData playerData)
    {
        Leaderboard.GetEntries(LeaderboardName, (result) =>
        {
            _hasResult = true;

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
        }, (error) =>
        {
            if (_hasResult == false)
            {
                StartCoroutine(WaitForSDKInitialization(playerData));
            }
        });
    }

    private IEnumerator WaitForSDKInitialization(PlayerData playerData)
    {
        while (YandexGamesSdk.IsInitialized == false)
        {
            yield return _waitForSDKInitializationCheck;
        }

        SetScore(playerData);
    }
}
