using System.Globalization;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private ScoreAllocator _scoreAllocator;
    [SerializeField] private float _scoreResetSpeed = .5f;
    [SerializeField] private bool _isDebug;
    [SerializeField] private TMP_Text _score;

    private readonly CultureInfo _cultureInfo = new CultureInfo("ru-RU");


    private void OnEnable()
    {
        _scoreAllocator.ScoreChanged += Display;
    }

    private void OnDisable()
    {
        _scoreAllocator.ScoreChanged -= Display;
    }

    public void Display(Score score)
    {
        SetScore(score.Current);

        if (_isDebug)
        {
            Debug.Log($"{score.Current} | {score.Change} | {score.Initiator}");
        }
    }

    public void SetScoreWithAnimation(float score)
    {
        int fromValue = int.Parse(_score.text.Replace(" ", string.Empty));
        int endValue = int.Parse(score.ToString(_cultureInfo));

        _score.DOCounter(fromValue, endValue, _scoreResetSpeed, true, _cultureInfo);
    }

    private void SetScore(float score)
    {
        _score.text = score.ToString("# ##0", _cultureInfo);
    }
}