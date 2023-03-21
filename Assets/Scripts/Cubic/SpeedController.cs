using System.Collections;
using UnityEngine;

public class SpeedController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _changeLineSpeed = .1f;
    [SerializeField] private float _stopAtPressStandSpeed = 1f;
    [SerializeField] private float _slowdownFactor = .2f;
    [SerializeField] private float _acceleration = .1f;
    [SerializeField] private BlockStacker _blockStacker;
    [SerializeField] private AnimationCurve _stopCurve;

    private float _currentSpeed;
    private float _initialSpeed;
    private float _startTime = 0;
    private float _runningTime;
    private bool _isMaxSpeed;
    private bool _isStop;

    public float CurrentSpeed => _currentSpeed;
    public float ChangeLineSpeed => _changeLineSpeed;
    public float StopAtPressStandSpeed => _stopAtPressStandSpeed;

    private void OnEnable()
    {
        _blockStacker.WrongBlockTaken += OnWrongBlockTaken;
    }

    private void Start()
    {
        _currentSpeed = _moveSpeed;
        _isMaxSpeed = true;
    }

    private void Update()
    {
        if(_currentSpeed < _moveSpeed && _isMaxSpeed == true)
        {
            _isMaxSpeed = false;
            StartCoroutine(Accelerate());
        }
    }

    private void OnDisable()
    {
        _blockStacker.WrongBlockTaken -= OnWrongBlockTaken;
    }

    public void SlowDown(bool isSlowStop)
    {
        if (isSlowStop)
        {
            _isStop = true;
            StartCoroutine(StopSlowly());
        }
        else
        {
            _currentSpeed -= _currentSpeed * _slowdownFactor;
            _runningTime = _startTime;
            _initialSpeed = _currentSpeed;
        }
    }

    private void OnWrongBlockTaken()
    {
        SlowDown(false);
    }

    private IEnumerator Accelerate()
    {
        while (_currentSpeed < _moveSpeed)
        {
            if(_isStop)
            {
                yield break;
            }

            _currentSpeed = _initialSpeed + _acceleration * _runningTime;
            _runningTime += Time.deltaTime;
            yield return null;
        }

        _currentSpeed = _moveSpeed;
        _isMaxSpeed = true;
    }

    private IEnumerator StopSlowly()
    {
        float runningTime = 0;
        float stopDuration;

        stopDuration = _stopCurve.keys[_stopCurve.length - 1].time;

        while (runningTime <= stopDuration)
        {
            _currentSpeed = _moveSpeed * _stopCurve.Evaluate(runningTime);
            runningTime += Time.deltaTime;
            yield return null;
        }
    }
}
