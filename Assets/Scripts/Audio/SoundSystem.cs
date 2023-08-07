using System.Collections.Generic;
using UnityEngine;

public class SoundSystem : MonoBehaviour
{
    [SerializeField] private List<SoundInfo> _soundTypes;
    [SerializeField] private AudioSource _audioSource;

    public void Play(SoundEvent soundEvent)
    {
        foreach (SoundInfo soundType in _soundTypes)
        {
            if (soundType.SoundEvent == soundEvent)
            {
                _audioSource.clip = soundType.Clip;
                _audioSource.volume = soundType.Volume;
                _audioSource.pitch = soundType.Pitch;
                _audioSource.Play();
            }
        }
    }
}

