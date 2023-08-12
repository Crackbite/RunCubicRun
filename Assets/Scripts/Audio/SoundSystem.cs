using System.Collections.Generic;
using UnityEngine;

public class SoundSystem : MonoBehaviour
{
    [SerializeField] private List<SoundInfo> _soundTypes;
    [SerializeField] private List<AudioSource> _audioSources;

    public void Play(SoundEvent soundEvent)
    {
        foreach (SoundInfo soundType in _soundTypes)
        {
            if (soundType.SoundEvent == soundEvent)
            {
                foreach (AudioSource audioSource in _audioSources)
                {
                    if (audioSource.isPlaying == false)
                    {
                        audioSource.clip = soundType.Clip;
                        audioSource.volume = soundType.Volume;
                        audioSource.pitch = soundType.Pitch;
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
        if (CheckSoundPlaying(soundEvent, out AudioSource audioSource))
        {
            audioSource.Stop();
        }
    }

    public bool CheckSoundPlaying(SoundEvent soundEvent, out AudioSource playingAudioSource)
    {
        foreach (SoundInfo soundType in _soundTypes)
        {
            if (soundType.SoundEvent == soundEvent)
            {
                foreach (AudioSource audioSource in _audioSources)
                {
                    if (audioSource.clip == soundType.Clip && audioSource.isPlaying)
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
}

