using DG.Tweening;
using UnityEngine;

public abstract class Screen : MonoBehaviour
{
    private DOTweenAnimation[] _tweenAnimations;

    private void Start()
    {
        _tweenAnimations = GetComponentsInChildren<DOTweenAnimation>();
    }

    public virtual void Enter()
    {
        gameObject.SetActive(true);
    }

    public virtual void Exit()
    {
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