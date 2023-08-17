using DG.Tweening;
using UnityEngine;

public class Soundtrack : MonoBehaviour
{
    [SerializeField] private AudioClip _menuAudio;
    [SerializeField] private AudioClip _gameplayAudio;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private GameStatusTracker _gameStatusTracker;
    [SerializeField] private SwitchToggle _musicSwitchToggle;
    [SerializeField] private SoundSystem _soundSystem;
    [SerializeField] private Cubic _cubic;
    [SerializeField] private float _fadeDuration;

    private bool _isMusicOn = true;

    private void OnEnable()
    {
        _gameStatusTracker.GameStarted += OnGameStarted;
        _gameStatusTracker.GameEnded += OnGameEnded;
        _musicSwitchToggle.ToggleChanged += OnMusicToggleChanged;
        _cubic.SteppedOnStand += OnCubicSteppedOnStend;
    }

    private void Awake()
    {
       Play(_menuAudio);
    }

    private void OnDisable()
    {
        _gameStatusTracker.GameStarted -= OnGameStarted;
        _gameStatusTracker.GameEnded -= OnGameEnded;
        _musicSwitchToggle.ToggleChanged -= OnMusicToggleChanged;
        _cubic.SteppedOnStand -= OnCubicSteppedOnStend;
    }

    private void Play(AudioClip clip)
    {
        if (_isMusicOn)
        {
            _audioSource.clip = clip;
            _audioSource.Play();
        }
    }

    private void FadeAudio()
    {
        const float TargetVolume = 0;

        _audioSource.DOFade(TargetVolume, _fadeDuration);
    }

    private void OnCubicSteppedOnStend(PressStand _)
    {
        FadeAudio();
    }

    private void OnGameEnded(GameResult result)
    {
        _audioSource.Stop();
    }

    private void OnGameStarted()
    {
        Play(_gameplayAudio);
    }

    private void OnMusicToggleChanged(bool isSoundOn)
    {
        _soundSystem.Play(SoundEvent.SwitchToggle);
        _isMusicOn = isSoundOn;

        if (_isMusicOn == false)
        {
            _audioSource.Pause();
        }
        else if(_audioSource.isPlaying == false)
        {
            _audioSource.Play();
        }
    }
}
