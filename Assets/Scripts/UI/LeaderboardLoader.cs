using UnityEngine;
using Agava.YandexGames;
using System;

public class LeaderboardLoader : MonoBehaviour
{
    [SerializeField] private SDK _sdk;
    [SerializeField] private ScoreAllocator _scoreAllocator;
    [SerializeField] private LeaderboardScreen _leaderboardScreen;
    [SerializeField] private AuthRequestScreen _authRequestScreen;

    private string _playerUniqueID;

    private const string LeaderboardName = "LeaderboardRCR";
    private const string AnonymousName = "Anonymous";

    private void OnEnable()
    {
        _sdk.Initialized += OnSDKInitialized;
        _scoreAllocator.ScoreChanged += OnScoreChanged;
        _authRequestScreen.PersonalDataPermissed += OnPersonalDataPermissed;
    }

    private void OnDisable()
    {
        _sdk.Initialized -= OnSDKInitialized;
        _authRequestScreen.PersonalDataPermissed -= OnPersonalDataPermissed;
    }

    private void OnPersonalDataPermissed()
    {
        GetPlayerEntries();
    }

    private void OnScoreChanged(Score score)
    {
        GetPlayerEntries();
        _scoreAllocator.ScoreChanged -= OnScoreChanged;
    }

    private void OnSDKInitialized()
    {
        PlayerAccount.GetProfileData((result) =>
        {
            _playerUniqueID = result.uniqueID;
        });

        Leaderboard.SetScore(LeaderboardName, (int)_scoreAllocator.TotalScore);
    }

    private void GetPlayerEntries()
    {
        Leaderboard.GetEntries(LeaderboardName, (result) =>
        {
            for (int i = 0; i < result.entries.Length; i++)
            {
                string name = result.entries[i].player.publicName;

                if (string.IsNullOrEmpty(name))
                    name = AnonymousName;

                bool isUser = result.entries[i].player.uniqueID == _playerUniqueID;

                _leaderboardScreen.AddPlayerView(i + 1, name, result.entries[i].score, isUser);
            }
        });
    }
}
