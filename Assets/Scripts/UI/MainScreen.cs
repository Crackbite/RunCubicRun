using TMPro;
using UnityEngine;

public class MainScreen : Screen
{
    [SerializeField] private TMP_Text _level;
    [SerializeField] private GameDataHandler _gameDataHandler;
    [SerializeField] private bool _isTraining;

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
        if (_isTraining == false)
        {
            _level.text += _gameDataHandler.Level.ToString();
        }
    }
}