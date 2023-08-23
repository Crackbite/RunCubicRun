using System;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardScreen : Screen
{
    [SerializeField] private GameObject _itemContainer;
    [SerializeField] private PlayerView _template;
    [SerializeField] private Button _close;

    public event Action CloseClicked;

    private void OnEnable()
    {
        _close.onClick.AddListener(OnCloseClicked);
    }

    private void OnDisable()
    {
        _close.onClick.RemoveListener(OnCloseClicked);
    }

    private void OnCloseClicked()
    {
       CloseClicked?.Invoke();
    }

    public void AddPlayerView(int playerNumber, string playerName, int playerScore, bool isUser = false)
    {
        PlayerView view = Instantiate(_template, _itemContainer.transform);
        view.Render(playerNumber, playerName, playerScore);
    }
}
