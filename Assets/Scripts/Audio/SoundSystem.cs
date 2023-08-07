using System.Collections.Generic;
using UnityEngine;

public class SoundSystem : MonoBehaviour
{
    [SerializeField] private List<SoundType> _soundTypes;

    public void Play(SoundEvent soundEvent)
    {
        foreach (SoundType soundType in _soundTypes)
        {
            if (soundType.SoundEvent == soundEvent)
            {
                soundType.Sound.Play();
            }
        }
    }
}

