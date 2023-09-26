using System;
using System.Collections;
using UnityEngine;

public class PauseSystem : MonoBehaviour
{
    [SerializeField] private float _slowdownDuration = 2f;
    [SerializeField] private float _accelerationDuration = 1f;

    private Coroutine _scaleRoutine;
    private Coroutine _allowUnpauseRoutine;
    private bool _isStopTimeScaling;
    private bool _isScalingTime;
    private float _initialTimeScale;
    private float _targetTimeScale;
    private float _elapsedTime;
    private float _scaleDuration;

    public event Action TimeChanged;
    public event Action TimeSlowing;
    public event Action TimeAccelerating;

    public bool CanEndTrainingPause { get; private set; } = true;

    public void SlowDownTime()
    {
        const float targetValue = 0f;
        const float initialValue = 1f;

        StartScaleRoutine(initialValue, targetValue, _slowdownDuration, TimeSlowing);
    }

    public void AccelerateTime()
    {
        const float targetValue = 1f;
        const float initialValue = 0f;

        StartScaleRoutine(initialValue, targetValue, _accelerationDuration, TimeAccelerating);
    }

    public void PauseGame()
    {
        TryStopScaleRoutine();
        Time.timeScale = 0;
    }

    public void UnpauseGame()
    {
        TryStopScaleRoutine();
        Time.timeScale = 1;
    }

    public void FocusGame()
    {
        AudioListener.pause = false;

        if (_isScalingTime)
        {
            _isStopTimeScaling = false;

            if (_initialTimeScale != _targetTimeScale)
            {
                _scaleRoutine = StartCoroutine(ScaleTime(_initialTimeScale, _targetTimeScale, _scaleDuration - _elapsedTime));
            }

            _allowUnpauseRoutine = StartCoroutine(AllowUnpause(_scaleDuration - _elapsedTime));
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void UnfocusGame()
    {
        AudioListener.pause = true;

        if (_isScalingTime)
        {
            if (_allowUnpauseRoutine != null)
            {
                StopCoroutine(_allowUnpauseRoutine);
            }

            CanEndTrainingPause = false;
            _initialTimeScale = Time.timeScale;
        }

        Time.timeScale = 0;
    }

    private void StartScaleRoutine(float initialValue, float targetValue, float duration, Action onStartCallback)
    {
        const float InitialElapsedTime = 0;

        _elapsedTime = InitialElapsedTime;
        _initialTimeScale = initialValue;
        _targetTimeScale = targetValue;
        onStartCallback.Invoke();
        _scaleRoutine = StartCoroutine(ScaleTime(initialValue, targetValue, duration));
        _isScalingTime = true;
        _isStopTimeScaling = false;
    }

    private void TryStopScaleRoutine()
    {
        if (_scaleRoutine != null)
        {
            _isStopTimeScaling = true;
            _scaleRoutine = null;
        }
    }

    private IEnumerator ScaleTime(float initialValue, float targetValue, float duration)
    {
        const float PauseScale = 0;

        _scaleDuration = duration;

        while (_elapsedTime < _scaleDuration && _isStopTimeScaling == false)
        {
            float interpolationRatio = _elapsedTime / _scaleDuration;
            float newTimeScale = Mathf.Lerp(initialValue, targetValue, interpolationRatio);
            Time.timeScale = newTimeScale;
            _elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        if (_isStopTimeScaling)
        {
            yield break;
        }

        Time.timeScale = targetValue;

        if (targetValue > PauseScale)
        {
            _isScalingTime = false;
        }

        TimeChanged?.Invoke();
    }

    private IEnumerator AllowUnpause(float timeBeforeAllowing)
    {
        yield return new WaitForSecondsRealtime(timeBeforeAllowing);
        CanEndTrainingPause = true;
    }
}