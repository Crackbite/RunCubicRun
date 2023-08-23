using System;
using UnityEngine;
using UnityEngine.UI;

public class MenuScreen : Screen
{
    [SerializeField] private Button _start;
    [SerializeField] private Button _store;
    [SerializeField] private Button _leaderboard;

    public event Action StartClicked;
    public event Action StoreClicked;
    public event Action LeaderboardClicked;

    private void OnEnable()
    {
        _start.onClick.AddListener(OnStartClicked);
        _store.onClick.AddListener(OnStoreClicked);
        _leaderboard.onClick.AddListener(OnLeaderboardClicked);
    }

    private void OnDisable()
    {
        _start.onClick.RemoveListener(OnStartClicked);
        _store.onClick.RemoveListener(OnStoreClicked);
        _leaderboard.onClick.RemoveListener(OnLeaderboardClicked);
    }

    private void OnStartClicked()
    {
        StartClicked?.Invoke();
    }

    private void OnStoreClicked()
    {
        StoreClicked?.Invoke();
    }

    private void OnLeaderboardClicked()
    {
        LeaderboardClicked?.Invoke();
    }
}