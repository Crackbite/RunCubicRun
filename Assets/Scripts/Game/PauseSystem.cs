using System;
using System.Collections;
using UnityEngine;

public class PauseSystem : MonoBehaviour
{
    [SerializeField] private float _slowdownDuration = 2f;
    [SerializeField] private float _accelerationDuration = 1f;


    private Coroutine _scaleRoutine;
    private Coroutine _allowUnpauseRoutine;
    private bool _isStop;
    private bool _isTrainingTime;
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
        if (_scaleRoutine != null)
        {
            _isStop = true;
            _scaleRoutine = null;
        }

        Time.timeScale = 0;
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1;
        _isStop = false;
    }

    private void StartScaleRoutine(float initialValue, float targetValue, float duration, Action onStartCallback)
    {
        const float InitialElapsedTime = 0;

        _elapsedTime = InitialElapsedTime;
        _initialTimeScale = initialValue;
        _targetTimeScale = targetValue;
        onStartCallback.Invoke();
        _scaleRoutine = StartCoroutine(ScaleTime(initialValue, targetValue, duration));
        _isTrainingTime = true;
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            AudioListener.pause = false;

            if (_isTrainingTime)
            {
                _isStop = false;

                if (_initialTimeScale != _targetTimeScale)
                {
                    _scaleRoutine = StartCoroutine(ScaleTime(_initialTimeScale, _targetTimeScale, _scaleDuration - _elapsedTime));
                }

                _allowUnpauseRoutine = StartCoroutine(AllowUnpause(_scaleDuration - _elapsedTime));
            }
            else
            {
                UnpauseGame();
            }
        }
        else
        {
            AudioListener.pause = true;

            if (_isTrainingTime)
            {
                if (_allowUnpauseRoutine != null)
                {
                    StopCoroutine(_allowUnpauseRoutine);
                }

                CanEndTrainingPause = false;
                _initialTimeScale = Time.timeScale;
            }

            PauseGame();
        }
    }

    private IEnumerator ScaleTime(float initialValue, float targetValue, float duration)
    {
        _scaleDuration = duration;

        while (_elapsedTime < _scaleDuration && _isStop == false)
        {
            float interpolationRatio = _elapsedTime / _scaleDuration;
            float newTimeScale = Mathf.Lerp(initialValue, targetValue, interpolationRatio);
            Time.timeScale = newTimeScale;
            _elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        if (_isStop)
        {
            yield break;
        }

        Time.timeScale = targetValue;

        if (targetValue > 0)
        {
            _isTrainingTime = false;
        }

        TimeChanged?.Invoke();
    }

    private IEnumerator AllowUnpause(float timeBeforeAllowing)
    {
        yield return new WaitForSecondsRealtime(timeBeforeAllowing);
        CanEndTrainingPause = true;
    }
}