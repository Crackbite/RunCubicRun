using TMPro;
using UnityEngine;

public class MainScreen : Screen
{
    [SerializeField] private TMP_Text _level;
    [SerializeField] private GameDataHandler _gameDataHandler;

    private void OnEnable()
    {
        _gameDataHandler.DataRestored += OnDataRestored;
    }

    private void OnDisable()
    {
        _gameDataHandler.DataRestored -= OnDataRestored;
    }

    private void OnDataRestored()
    {
        _level.text += _gameDataHandler.Level.ToString();
    }
}