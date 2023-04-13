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
    [SerializeField] private AnimationCurve _fightSlowCurve;
    [SerializeField] private LevelEntryPortal _levelEntryPortal;

    private Cubic _cubic;

    private float _initialSpeed;
    private bool _isMaxSpeed;
    private bool _isThrowing;
    private bool _isStop;
    private float _runningTime;

    public float CurrentSpeed { get; private set; }
    public float StopAtPressStandSpeed => _stopAtPressStandSpeed;

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
        if (CurrentSpeed < _moveSpeed && _isMaxSpeed && _isThrowing == false)
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

    public void SlowDown()
    {
        const float StartTime = 0f;

        if (_cubic.IsSawing)
        {
            _isStop = true;
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
        StartCoroutine(StopSlowly(_fightSlowCurve));
    }

    private IEnumerator Accelerate()
    {
        while (CurrentSpeed < _moveSpeed)
        {
            if (_isStop)
            {
                yield break;
            }

            CurrentSpeed = _initialSpeed + (_acceleration * _runningTime);
            _runningTime += Time.deltaTime;

            yield return null;
        }

        CurrentSpeed = _moveSpeed;
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

        if(_isStop == false)
        {
            _isMaxSpeed = true;
            _isThrowing = false;
        }
        else
        {
            _cubic.SplitIntoPieces();
        }
    }
}