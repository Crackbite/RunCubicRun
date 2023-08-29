using DG.Tweening;
using UnityEngine;

public class Soundtrack : MonoBehaviour
{
    [SerializeField] private AudioClip _menuAudio;
    [SerializeField] private AudioClip _gameplayAudio;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private GameStatusTracker _gameStatusTracker;
    [SerializeField] private MenuScreen _menuScreen;
    [SerializeField] private SwitchToggle _musicSwitchToggle;
    [SerializeField] private SoundSystem _soundSystem;
    [SerializeField] private PauseSystem _pauseSystem;
    [SerializeField] private Cubic _cubic;
    [SerializeField] private GameDataHandler _gameDataHandler;
    [SerializeField] private float _fadeDuration;

    private bool _isMusicOn = true;
    private float _initialVolume;

    private void OnEnable()
    {
        _gameStatusTracker.GameStarted += OnGameStarted;
        _gameStatusTracker.GameEnded += OnGameEnded;
        _menuScreen.Set += OnMenuScreenSet;
        _musicSwitchToggle.ToggleChanged += OnMusicToggleChanged;
        _cubic.SteppedOnStand += OnCubicSteppedOnStend;
        _pauseSystem.TimeSlowing += OnTimeSlowing;
        _pauseSystem.TimeAccelerating += OnTimeAccelerating;
        _gameDataHandler.DataRestored += OnGameDataRestored;
    }

    private void Awake()
    {
        _initialVolume = _audioSource.volume;
    }

    private void OnDisable()
    {
        _gameStatusTracker.GameStarted -= OnGameStarted;
        _gameStatusTracker.GameEnded -= OnGameEnded;
        _menuScreen.Set -= OnMenuScreenSet;
        _musicSwitchToggle.ToggleChanged -= OnMusicToggleChanged;
        _cubic.SteppedOnStand -= OnCubicSteppedOnStend;
        _pauseSystem.TimeSlowing -= OnTimeSlowing;
        _pauseSystem.TimeAccelerating -= OnTimeAccelerating;
        _gameDataHandler.DataRestored -= OnGameDataRestored;
    }

    private void Play(AudioClip clip)
    {
        _audioSource.clip = clip;

        if (_isMusicOn)
        {
            _audioSource.Play();
        }
    }

    private void CheckMusicOn()
    {
        if (_isMusicOn == false)
        {
            _audioSource.Pause();
        }
        else if (_audioSource.isPlaying == false)
        {
            _audioSource.Play();
        }
    }

    private void FadeAudio(float targetVolume)
    {
        _audioSource.DOFade(targetVolume, _fadeDuration);
    }

    private void OnCubicSteppedOnStend(PressStand _)
    {
        const float OnStandVolume = 0;

        FadeAudio(OnStandVolume);
    }

    private void OnGameEnded(GameResult result)
    {
        _audioSource.Stop();
    }

    private void OnGameStarted()
    {
        Play(_gameplayAudio);
    }

    private void OnMenuScreenSet()
    {
        if (_audioSource.isPlaying == false || _audioSource.clip != _menuAudio)
        {
            Play(_menuAudio);
        }
    }

    private void OnMusicToggleChanged(bool isMusicOn, SwitchToggle _)
    {
        _soundSystem.Play(SoundEvent.SwitchToggle);
        _isMusicOn = isMusicOn;
        CheckMusicOn();
    }

    private void OnTimeSlowing()
    {
        const float SlowedVolumePercent = 0.1f;

        FadeAudio(SlowedVolumePercent * _initialVolume);
    }

    private void OnTimeAccelerating()
    {
        FadeAudio(_initialVolume);
    }

    private void OnGameDataRestored()
    {
        _isMusicOn = _gameDataHandler.IsMusicOn;
        CheckMusicOn();
    }
}
