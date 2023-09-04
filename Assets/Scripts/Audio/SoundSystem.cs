using System.Collections.Generic;
using UnityEngine;

public class SoundSystem : MonoBehaviour
{
    [SerializeField] private List<SoundInfo> _soundInfoList;
    [SerializeField] private List<AudioSource> _audioSources;
    [SerializeField] private SwitchToggle _soundSwitchToggle;
    [SerializeField] private AuthRequestScreen _authRequestScreen;

    private bool _isSoundOn = true;
    private bool _isFirstTime = true;

    private void OnEnable()
    {
        _soundSwitchToggle.ToggleChanged += OnSoundToggleChanged;
        _authRequestScreen.PlayerAuthorized += OnPlayerAuthorized;
    }

    private void OnDisable()
    {
        _soundSwitchToggle.ToggleChanged -= OnSoundToggleChanged;
        _authRequestScreen.PlayerAuthorized -= OnPlayerAuthorized;
    }

    public void Play(SoundEvent soundEvent)
    {
        if (_isSoundOn == false)
        {
            return;
        }

        foreach (SoundInfo soundInfo in _soundInfoList)
        {
            if (soundInfo.SoundEvent == soundEvent)
            {
                foreach (AudioSource audioSource in _audioSources)
                {
                    if (audioSource.isPlaying == false)
                    {
                        audioSource.clip = soundInfo.Clip;
                        audioSource.volume = soundInfo.Volume;
                        audioSource.pitch = soundInfo.Pitch;
                        audioSource.Play();
                        return;
                    }
                }

                return;
            }
        }
    }

    public void Stop(SoundEvent soundEvent)
    {
        if (_isSoundOn == false)
        {
            return;
        }

        if (CheckSoundPlaying(soundEvent, out AudioSource audioSource))
        {
            audioSource.Stop();
        }
    }

    public bool CheckSoundPlaying(SoundEvent soundEvent, out AudioSource playingAudioSource)
    {
        foreach (SoundInfo soundInfo in _soundInfoList)
        {
            if (soundInfo.SoundEvent == soundEvent)
            {
                foreach (AudioSource audioSource in _audioSources)
                {
                    if (audioSource.clip == soundInfo.Clip && audioSource.isPlaying)
                    {
                        playingAudioSource = audioSource;
                        return true;
                    }
                }
            }
        }

        playingAudioSource = null;
        return false;
    }

    private void OnSoundToggleChanged(bool isSoundOn, SwitchToggle _)
    {
        _isSoundOn = isSoundOn;

        if (_isFirstTime == false)
        {
            Play(SoundEvent.SwitchToggle);
        }
        else
        {
            _isFirstTime = false;
        }
    }

    private void OnPlayerAuthorized()
    {
        _isFirstTime = true;
    }
}

