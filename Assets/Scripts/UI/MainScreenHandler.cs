using UnityEngine;

public class MainScreenHandler : MonoBehaviour
{
    [SerializeField] private MainScreen _mainScreen;
    [SerializeField] private DataRestorer _dataRestorer;
    [SerializeField] private AuthRequestScreen _authRequestScreen;
    [SerializeField] private LevelSetter _levelSetter;
    [SerializeField] private ScoreAllocator _scoreAllocator;
    [SerializeField] private ScoreDisplay _scoreDisplay;

    private PlayerData _playerData;
    private Score _score;
    private bool _wasAuth;
    private bool _canSetScore;

    private void OnEnable()
    {
        _dataRestorer.DataRestored += OnDataRestored;
        _authRequestScreen.AuthClicked += OnAuthClicked;
        _levelSetter.Set += OnLevelSet;
        _scoreAllocator.ScoreChanged += OnScoreChanged;
    }

    private void OnDisable()
    {
        _dataRestorer.DataRestored -= OnDataRestored;
        _authRequestScreen.AuthClicked -= OnAuthClicked;
        _levelSetter.Set -= OnLevelSet;
        _scoreAllocator.ScoreChanged -= OnScoreChanged;
        _mainScreen.Hidden -= OnHidden;
    }

    private void OnDataRestored(PlayerData playerData)
    {
        _playerData = playerData;

        if (_wasAuth)
        {
            return;
        }

        _levelSetter.SetLevel(playerData);
    }

    private void OnAuthClicked()
    {
        _mainScreen.Hidden += OnHidden;
        _wasAuth = true;
        _mainScreen.Exit();
    }

    private void OnHidden()
    {
        _mainScreen.Hidden -= OnHidden;
        _wasAuth = false;
        _levelSetter.SetLevel(_playerData);
    }

    private void OnLevelSet()
    {
        if (_score != null)
        {
            _scoreDisplay.Display(_score);
        }
        else
        {
            _canSetScore = true;
        }

        _mainScreen.Enter();
    }

    private void OnScoreChanged(Score score)
    {
        _score = score;

        if (_canSetScore)
        {
            _scoreDisplay.Display(_score);
            _canSetScore = false;
        }
    }
}
