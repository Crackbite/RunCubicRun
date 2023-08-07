using UnityEngine;

[System.Serializable]
public class SoundType
{
    [SerializeField] private AudioSource _sound;
    [SerializeField] private SoundEvent _soundEvent;

    public AudioSource Sound => _sound;
    public SoundEvent SoundEvent => _soundEvent;
}

