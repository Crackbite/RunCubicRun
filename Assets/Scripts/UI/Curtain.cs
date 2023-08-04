using DG.Tweening;
using UnityEngine;

public class Curtain : MonoBehaviour
{
    [SerializeField] private GameObject _curtain;
    [SerializeField] private PressTopAnimator _pressTopAnimator;
    [SerializeField] private Cubic _cubic;
    [SerializeField] private PistonMover _pistonMover;
    [SerializeField] private CubicMovement _cubicMovement;
    [SerializeField] private DOTweenAnimation _moveAnimation;

    private void OnEnable()
    {
        _cubic.SteppedOnStand += OnCubicSteppedOnStand;
        _cubicMovement.CubicLeftPress += OnCubicLeftPress;
        _pressTopAnimator.Completed += OnPressTopAnimationCompleted;
        _pistonMover.WorkCompleted += OnPistonMoverWorkCompleted;
    }

    private void OnDisable()
    {
        _cubic.SteppedOnStand -= OnCubicSteppedOnStand;
        _cubicMovement.CubicLeftPress -= OnCubicLeftPress;
        _pressTopAnimator.Completed -= OnPressTopAnimationCompleted;
        _pistonMover.WorkCompleted += OnPistonMoverWorkCompleted;
    }

    private void OnCubicSteppedOnStand(PressStand pressStand)
    {
        Invoke(nameof(Show), 2f);
    }

    private void OnCubicLeftPress()
    {
        Hide();
    }

    private void OnPressTopAnimationCompleted()
    {
        Show();
    }

    private void OnPistonMoverWorkCompleted()
    {
        Hide();
    }

    private void Show()
    {
        _curtain.SetActive(true);
    }

    private void Hide()
    {
        _moveAnimation.DOPlayBackwards();
        _moveAnimation.tween.OnRewind(() => _curtain.SetActive(false));
    }
}
