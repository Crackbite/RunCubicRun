using UnityEngine;

public class Soundtrack : MonoBehaviour
{
    [SerializeField] private AudioSource _menuAudio;
    [SerializeField] private AudioSource _gameplayAudio;
    [SerializeField] private GameStatusTracker _gameStatusTracker;
    [SerializeField] private LevelEntryPortal _levelEntryPortal;

    private void OnEnable()
    {
        _gameStatusTracker.GameStarted += OnGameStarted;
        _gameStatusTracker.GameEnded += OnGameEnded;
    }

    private void Awake()
    {
        _menuAudio.Play();
    }

    private void OnDisable()
    {
        _gameStatusTracker.GameStarted -= OnGameStarted;
        _gameStatusTracker.GameEnded -= OnGameEnded;
    }

    private void OnGameEnded(GameResult result)
    {
        _gameplayAudio.Stop();
    }

    private void OnGameStarted()
    {
        _menuAudio.Stop();
        _gameplayAudio.Play();
    }
}
