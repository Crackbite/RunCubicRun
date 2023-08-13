using System.Collections.Generic;
using UnityEngine;

public class SoundSystem : MonoBehaviour
{
    [SerializeField] private List<SoundInfo> _soundInfoList;
    [SerializeField] private List<AudioSource> _audioSources;

    public void Play(SoundEvent soundEvent)
    {
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
}

