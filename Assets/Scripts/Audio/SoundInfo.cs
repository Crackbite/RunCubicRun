using UnityEngine;

[System.Serializable]
public class SoundInfo
{
    [Range(0f, 1f)] 
    [SerializeField] private float _volume;
    [Range(0f, 1f)]
    [SerializeField] private float _pitch;
    [SerializeField] private AudioClip _clip;
    [SerializeField] private SoundEvent _soundEvent;

    public float Volume => _volume;
    public float Pitch => _pitch;
    public AudioClip Clip => _clip;
    public SoundEvent SoundEvent => _soundEvent;
}

