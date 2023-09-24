using UnityEngine;
using Agava.YandexGames;
using System.Collections;
using System;
using Lean.Localization;
using System.Xml;

public class LeaderboardLoader : MonoBehaviour
{
    [SerializeField] private DataRestorer _dataRestorer;
    [SerializeField] private LeaderboardScreen _leaderboardScreen;
    [SerializeField] private GameObject _anonymousPhrase;

    private WaitForSecondsRealtime _waitForSDKInitializationCheck;
    private bool _hasResult;
    private string _anonymousName;
    private string _playerUniqueID;

    private const string LeaderboardName = "Leaderboard";

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

        _anonymousName = LeanLocalization.GetTranslationText(_anonymousPhrase.name);

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
                    name = _anonymousName;
                }

                int rank = result.entries[i].rank;
                bool isPlayer = result.entries[i].player.uniqueID == _playerUniqueID ? true : false;
                _leaderboardScreen.AddPlayerView(rank, name, result.entries[i].score, isPlayer);
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

        PlayerAccount.GetProfileData((result) =>
        {
            _playerUniqueID = result.uniqueID;
            SetScore(playerData);
        });
    }
}
