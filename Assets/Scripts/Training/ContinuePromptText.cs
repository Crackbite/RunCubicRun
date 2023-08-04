using System.Collections;
using TMPro;
using UnityEngine;

public class ContinuePromptText : MonoBehaviour
{
    [SerializeField] private TMP_Text _continuePromptText;
    [SerializeField] private float _blinkInterval = 0.5f;
    [SerializeField] private float _fadeInDuration = 0.5f;
    [SerializeField] private float _fadeOutDuration = 0.5f;

    private float _fadeOutAlpha = 0f;
    private float _fadeInAlpha = 1f;
    private bool _isBlinking;

    private void OnEnable()
    {
        StartBlinking();
    }

    private void Awake()
    {
        _continuePromptText.CrossFadeAlpha(_fadeOutAlpha, 0, true);
    }

    private void OnDisable()
    {
        StopBlinking();
    }

    private void StartBlinking()
    {
        if (_isBlinking == false)
        {
            _isBlinking = true;
            StartCoroutine(BlinkText());
        }
    }

    private void StopBlinking()
    {
        if (_isBlinking)
        {
            _continuePromptText.CrossFadeAlpha(_fadeOutAlpha, 0, true);
            _isBlinking = false;
        }
    }

    private IEnumerator BlinkText()
    {
        while (_isBlinking)
        {
            _continuePromptText.CrossFadeAlpha(_fadeInAlpha, _fadeInDuration, true);
            yield return new WaitForSecondsRealtime(_blinkInterval);
            _continuePromptText.CrossFadeAlpha(_fadeOutAlpha, _fadeOutDuration, true);
            yield return new WaitForSecondsRealtime(_blinkInterval);
        }
    }
}
