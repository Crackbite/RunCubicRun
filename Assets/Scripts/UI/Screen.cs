using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public abstract class Screen : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    private DOTweenAnimation[] _tweenAnimations;

    public event Action Hidden;
    public event Action Showed;

    public IReadOnlyCollection<DOTweenAnimation> TweenAnimations => _tweenAnimations;

    private void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _tweenAnimations = GetComponentsInChildren<DOTweenAnimation>();
    }

    public virtual void Enter()
    {
        if (_canvasGroup != null)
        {
            _canvasGroup.interactable = true;
        }

        Show();
    }

    public virtual void Exit()
    {
        if (_canvasGroup != null)
        {
            _canvasGroup.interactable = false;
        }

        if (_tweenAnimations != null && _tweenAnimations.Length > 0)
        {
            foreach (DOTweenAnimation doTweenAnimation in _tweenAnimations)
            {
                doTweenAnimation.DOPlayBackwards();
            }

            _tweenAnimations[0].tween.OnRewind(Hide);
        }
        else
        {
            Hide();
        }
    }

    private void Hide()
    {
        gameObject.SetActive(false);
        Hidden?.Invoke();
    }

    private void Show()
    {
        gameObject.SetActive(true);

        if (this is not GameScreen)
        {
            StartCoroutine(WaitForShowingToComplete());
        }
    }

    private IEnumerator WaitForShowingToComplete()
    {
        while (_tweenAnimations == null)
        {
            yield return null;
        }

        if (_tweenAnimations[0] != null)
        {
            _tweenAnimations[0].tween.OnComplete(() => Showed?.Invoke());
        }
        else
        {
            Showed?.Invoke();
        }
    }
}