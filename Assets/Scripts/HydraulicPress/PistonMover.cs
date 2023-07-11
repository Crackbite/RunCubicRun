using System;
using UnityEngine;

[RequireComponent(typeof(PressSpeedHandler), typeof(PistonPresser))]
public class PistonMover : MonoBehaviour
{
    [SerializeField] private PressStand _pressStand;
    [SerializeField] private ParticleSystem _crushEffect;

    private bool _isCubicReached;

    private Cubic _cubic;
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
    }

    private void Update()
    {
        const float Threshold = 0.001f;

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
                IsWorking = false;
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
        _pistonPresser.CubicReached -= PistonPresserOnCubicReached;
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
    }

    private void PistonPresserOnCubicReached(Cubic cubic)
    {
        _cubic = cubic;
        _cubic.FlattenedOut += OnCubicFlattenedOut;
        _isCubicReached = true;
    }

    private void OnCubicFlattenedOut()
    {
        _cubic.FlattenedOut -= OnCubicFlattenedOut;
        IsWorking = false;
        WorkCompleted?.Invoke();
    }
}