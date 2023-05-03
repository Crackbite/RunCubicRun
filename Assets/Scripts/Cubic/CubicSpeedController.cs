using System;

using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Cubic))]
public class CubicSpeedController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _stopAtPressStandSpeed = 1f;
    [SerializeField] private float _slowdownFactor = .25f;
    [SerializeField] private float _acceleration = 3f;
    [SerializeField] private BlockStacker _blockStacker;
    [SerializeField] private AnimationCurve _stopCurve;
    [SerializeField] private AnimationCurve _throwSpeedCurve;
    [SerializeField] private LevelEntryPortal _levelEntryPortal;

    private Cubic _cubic;
    private float _initialSpeed;
    private bool _isMaxSpeed;
    private bool _isStop;
    private bool _isThrowing;
    private float _runningTime;

    public event Action CubicStopped;

    public float CurrentSpeed { get; private set; }

    public float SpeedMultiplier { get; private set; } = 1f;

    public float StopAtPressStandSpeed => _stopAtPressStandSpeed;

    private float Speed => _moveSpeed * SpeedMultiplier;

    private void OnEnable()
    {
        _blockStacker.WrongBlockTaken += OnWrongBlockTaken;
        _levelEntryPortal.ThrowingOut += OnCubicThrowingOut;
    }

    private void Start()
    {
        _cubic = GetComponent<Cubic>();

        CurrentSpeed = 0;
        _isThrowing = true;
    }

    private void Update()
    {
        if (CurrentSpeed < Speed && _isMaxSpeed && _isThrowing == false)
        {
            _isMaxSpeed = false;
            StartCoroutine(Accelerate());
        }
    }

    private void OnDisable()
    {
        _blockStacker.WrongBlockTaken -= OnWrongBlockTaken;
        _levelEntryPortal.ThrowingOut -= OnCubicThrowingOut;
    }

    public void SetSpeedMultiplier(float value)
    {
        const float MinSpeedMultiplier = 1f;

        value = value < MinSpeedMultiplier ? MinSpeedMultiplier : value;
        SpeedMultiplier = value;

        if (CurrentSpeed > Speed)
        {
            CurrentSpeed = Speed;
        }
    }

    public void SlowDown()
    {
        const float StartTime = 0f;

        if (_cubic.IsSawing)
        {
            _isStop = true;
            _initialSpeed = CurrentSpeed;
            StartCoroutine(StopSlowly(_stopCurve));
        }
        else
        {
            CurrentSpeed -= CurrentSpeed * _slowdownFactor;
            _runningTime = StartTime;
            _initialSpeed = CurrentSpeed;
        }
    }

    private void OnCubicThrowingOut()
    {
        _initialSpeed = _moveSpeed;
        StartCoroutine(StopSlowly(_throwSpeedCurve));
    }

    private IEnumerator Accelerate()
    {
        while (CurrentSpeed < Speed)
        {
            if (_isStop)
            {
                yield break;
            }

            CurrentSpeed = _initialSpeed + (_acceleration * _runningTime);
            _runningTime += Time.deltaTime;

            yield return null;
        }

        CurrentSpeed = Speed;
        _isMaxSpeed = true;
    }

    private void OnWrongBlockTaken()
    {
        SlowDown();
    }

    private IEnumerator StopSlowly(AnimationCurve slowCurve)
    {
        float runningTime = 0;
        float stopDuration = slowCurve.keys[slowCurve.length - 1].time;

        while (runningTime <= stopDuration)
        {
            CurrentSpeed = _initialSpeed * slowCurve.Evaluate(runningTime);
            runningTime += Time.deltaTime;
            yield return null;
        }

        if (_isStop == false)
        {
            _isMaxSpeed = true;
            _isThrowing = false;
            CurrentSpeed = _moveSpeed;
        }
        else
        {
            if (_cubic.CollisionSaw is VerticalSaw)
            {
                _cubic.SplitIntoPieces();
            }

            CurrentSpeed = 0;
            CubicStopped?.Invoke();
        }
    }
}