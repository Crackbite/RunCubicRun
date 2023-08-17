using UnityEngine;

public class Soundtrack : MonoBehaviour
{
    [SerializeField] private AudioClip _menuAudio;
    [SerializeField] private AudioClip _gameplayAudio;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private GameStatusTracker _gameStatusTracker;
    [SerializeField] private LevelEntryPortal _levelEntryPortal;

    private void OnEnable()
    {
        _gameStatusTracker.GameStarted += OnGameStarted;
        _gameStatusTracker.GameEnded += OnGameEnded;
    }

    private void Awake()
    {
       Play(_menuAudio);
    }

    private void OnDisable()
    {
        _gameStatusTracker.GameStarted -= OnGameStarted;
        _gameStatusTracker.GameEnded -= OnGameEnded;
    private void Play(AudioClip clip)
    {
        if (_isMusicOn)
        {
            _audioSource.clip = clip;
            _audioSource.Play();
        }
    }
    }

    private void OnGameEnded(GameResult result)
    {
        _gameplayAudio.Stop();
    }

    private void OnGameStarted()
    {
        Play(_gameplayAudio);
    }
    }
}
