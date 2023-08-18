using System;
using UnityEngine;

[RequireComponent(typeof(PressSpeedHandler), typeof(PistonPresser))]
public class PistonMover : MonoBehaviour
{
    [SerializeField] private PressStand _pressStand;
    [SerializeField] private ParticleSystem _crushEffect;
    [SerializeField] private Cubic _cubic;

    private bool _isCubicReached;
    const float Threshold = 0.001f;

    private PistonPresser _pistonPresser;
    private PressSpeedHandler _pressSpeedHandler;

    public event Action WorkCompleted;

    public bool IsWorking { get; private set; }

    private void Awake()
    {
        _pistonPresser = GetComponent<PistonPresser>();
        _pressSpeedHandler = GetComponent<PressSpeedHandler>();
    }

    private void OnEnable()
    {
        _pistonPresser.CubicReached += PistonPresserOnCubicReached;
        _pistonPresser.BlockDestroyed += PistonPresserOnBlockDestroyed;
        _cubic.FlattenedOut += OnCubicFlattenedOut;
    }

    private void Update()
    {
        if (IsWorking == false)
        {
            return;
        }

        Vector3 currentPosition = transform.position;
        Vector3 targetPosition = currentPosition;

        if (_isCubicReached == false)
        {
            targetPosition.y = _pressStand.Bounds.max.y;
            float speed = _pressSpeedHandler.GetCurrentSpeed() * Time.deltaTime;
            transform.position = Vector3.MoveTowards(currentPosition, targetPosition, speed);

            if (Mathf.Abs(currentPosition.y - targetPosition.y) < Threshold)
            {
                TurnOff();
                _cubic.SoundSystem.Stop(SoundEvent.PressHum);
                WorkCompleted?.Invoke();
            }
        }
        else
        {
            targetPosition.y = _cubic.Bounds.max.y;
            transform.position = targetPosition;
        }
    }

    private void OnDisable()
    {
        _cubic.FlattenedOut -= OnCubicFlattenedOut;
        _pistonPresser.CubicReached -= PistonPresserOnCubicReached;
        _pistonPresser.BlockDestroyed -= PistonPresserOnBlockDestroyed;
    }

    public void Init()
    {
        _pressSpeedHandler.Init();
    }

    public void TurnOff()
    {
        IsWorking = false;
    }

    public void TurnOn()
    {
        IsWorking = true;
        bool isPressHumPlaying = _cubic.SoundSystem.CheckSoundPlaying(SoundEvent.PressHum, out AudioSource _);

        if (isPressHumPlaying == false)
        {
            _cubic.SoundSystem.Play(SoundEvent.PressHum);
        }
    }

    private void PistonPresserOnCubicReached()
    {
        _isCubicReached = true;
    }

    private void PistonPresserOnBlockDestroyed()
    {
        _cubic.SoundSystem.Play(SoundEvent.BlockCrush);
    }

    private void OnCubicFlattenedOut()
    {
        _crushEffect.transform.position = _cubic.transform.position;
        _crushEffect.Play();
        IsWorking = false;
        WorkCompleted?.Invoke();
    }
}