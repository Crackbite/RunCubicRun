using System.Collections.Generic;
using System.Globalization;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class ScoreAnimation : MonoBehaviour
{
    [SerializeField] private TMP_Text _score;
    [SerializeField] private ScoreAllocator _scoreAllocator;
    [SerializeField] private DOTweenAnimation _containerAnimation;
    [SerializeField] private float _scoreSpeed = 1f;
    [SerializeField] private bool _fromZero;

    private void Start()
    {
        List<Tween> tweens = _containerAnimation.GetTweens();

        if (tweens.Count < 1)
        {
            _score.text = _fromZero ? _scoreAllocator.ToString(_scoreAllocator.TotalScore) : "0";
            return;
        }

        tweens[0].OnComplete(OnContainerShow);
        _score.text = _fromZero ? _scoreAllocator.ToString(_scoreAllocator.TotalScore)
            : _scoreAllocator.ToString(_scoreAllocator.LevelScore);
    }

    private void OnContainerShow()
    {
        int fromValue = (int)_scoreAllocator.LevelScore;
        int endValue = 0;

        if (_fromZero)
        {
            fromValue = (int)_scoreAllocator.TotalScore;
            endValue = (int)_scoreAllocator.TotalScore + (int)_scoreAllocator.LevelScore;
        }

        _score.DOCounter(fromValue, endValue, _scoreSpeed, true, new CultureInfo("ru-RU"));
    }
}