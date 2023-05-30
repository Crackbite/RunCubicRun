using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public abstract class Screen : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    private DOTweenAnimation[] _tweenAnimations;

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

        gameObject.SetActive(true);
    }

    public virtual void Exit()
    {
        if (_canvasGroup != null)
        {
            _canvasGroup.interactable = false;
        }

        if (_tweenAnimations.Length > 0)
        {
            foreach (DOTweenAnimation doTweenAnimation in _tweenAnimations)
            {
                doTweenAnimation.DOPlayBackwards();
            }

            _tweenAnimations[0].tween.OnRewind(() => gameObject.SetActive(false));
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}