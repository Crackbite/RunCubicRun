
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

    private Cubic _cubic;

    private float _initialSpeed;
    private bool _isMaxSpeed;
    private bool _isStop;
    private float _runningTime;

    public float CurrentSpeed { get; private set; }
    public float StopAtPressStandSpeed => _stopAtPressStandSpeed;

    private void OnEnable()
    {
        _blockStacker.WrongBlockTaken += OnWrongBlockTaken;
    }

    private void Start()
    {
        _cubic = GetComponent<Cubic>();

        CurrentSpeed = _moveSpeed;
        _isMaxSpeed = true;
    }

    private void Update()
    {
        if (CurrentSpeed < _moveSpeed && _isMaxSpeed)
        {
            _isMaxSpeed = false;
            StartCoroutine(Accelerate());
        }
    }

    private void OnDisable()
    {
        _blockStacker.WrongBlockTaken -= OnWrongBlockTaken;
    }

    public void SlowDown()
    {
        const float StartTime = 0f;

        if (_cubic.IsSawing)
        {
            _isStop = true;
            StartCoroutine(StopSlowly());
        }
        else
        {
            CurrentSpeed -= CurrentSpeed * _slowdownFactor;
            _runningTime = StartTime;
            _initialSpeed = CurrentSpeed;
        }
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

    private IEnumerator StopSlowly()
    {
        float runningTime = 0;
        float stopDuration = _stopCurve.keys[_stopCurve.length - 1].time;

        while (runningTime <= stopDuration)
        {
            CurrentSpeed = _moveSpeed * _stopCurve.Evaluate(runningTime);
            runningTime += Time.deltaTime;

            yield return null;
        }
    }
}