using TMPro;
using UnityEngine;

public class MainScreen : Screen
{
    [SerializeField] private TMP_Text _level;
    [SerializeField] private GameDataHandler _gameDataHandler;

    private void Start()
    {
        _level.text += _gameDataHandler.Level.ToString();
    }
}