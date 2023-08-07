using System;
using System.Collections;
using UnityEngine;

public class PauseSystem : MonoBehaviour
{
    [SerializeField] private float _slowdownDuration = 2f;
    [SerializeField] private float _resetDuration = 1f;
    [SerializeField] private SettingsScreen _settingsScreen;

    private Coroutine _scaleRoutine;
    private bool _isStop;

    public event Action TimeChanged;

    private void OnEnable()
    {
        _settingsScreen.Showed += OnSettingsShowed;
        _settingsScreen.Hidden += OnSettingsHidden;
    }

    private void OnDisable()
    {
        _settingsScreen.Showed -= OnSettingsShowed;
        _settingsScreen.Hidden -= OnSettingsHidden;
    }

    public void SlowDownTime()
    {
        const float targetValue = 0f;
        const float initialValue = 1f;

        _scaleRoutine = StartCoroutine(ScaleTime(initialValue, targetValue, _slowdownDuration));
    }

    public void AccelerateTime()
    {
        const float targetValue = 1f;
        const float initialValue = 0f;

        StartCoroutine(ScaleTime(initialValue, targetValue, _resetDuration));
    }

    private void OnSettingsHidden()
    {
        UnpauseGame();
    }

    private void OnSettingsShowed()
    {
        PauseGame();
    }

    private void PauseGame()
    {
        if (_scaleRoutine != null)
        {
            _isStop = true;
            _scaleRoutine = null;
        }

        Time.timeScale = 0;
    }

    private void UnpauseGame()
    {
        Time.timeScale = 1;
        _isStop = false;
    }

    private IEnumerator ScaleTime(float initialValue, float targetValue, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration && _isStop == false)
        {
            float interpolationRatio = elapsedTime / duration;
            float newTimeScale = Mathf.Lerp(initialValue, targetValue, interpolationRatio);
            Time.timeScale = newTimeScale;
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        if (_isStop)
        {
            yield break;
        }

        Time.timeScale = targetValue;
        TimeChanged?.Invoke();
    }
}