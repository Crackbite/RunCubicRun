using DG.Tweening;
using System;
using UnityEngine;

public class LevelExitPortal : HyperspacePortal
{
    [SerializeField] private CubicMovement _cubicMovement;
    [SerializeField] private Ease _dragEase = Ease.InCubic;
    [SerializeField] private PistonMover _pistonMover;
    [SerializeField] private PressStand _pressStand;


    private float _dragDistance = 5f;
    private float _dragDuration = 3f;
    private bool _isCubicLeft;

    public event Action SuckingIn;

    private void OnEnable()
    {
        _cubicMovement.CubicLeftPress += OnCubicLeftPress;
        _pistonMover.WorkCompleted += OnPistonWorkCompleted;
    }

    private void OnDisable()
    {
        _cubicMovement.CubicLeftPress -= OnCubicLeftPress;
        _pistonMover.WorkCompleted -= OnPistonWorkCompleted;
    }

    private void OnCubicLeftPress()
    {
        _isCubicLeft = true;
        Invoke(nameof(SuckIn), 1);
    }

    private void SuckIn()
    {
        SuckingIn?.Invoke();
        TargetPositionY = Center.position.y;
        TargetScale = Vector3.zero;
        Sequence dragSequence = DOTween.Sequence();

        dragSequence.Append(CubicTransform.DOBlendableMoveBy(new Vector3(_dragDistance, 0, 0), _dragDuration).SetEase(_dragEase))
            .OnComplete(() =>
            {
                FlightSequence = DOTween.Sequence();
                FlightSequence.Append(CubicTransform.DOMove(Center.position, FlightDuration).SetEase(FlightEase)).SetSpeedBased(true);
                FlightSequence.Join(CubicTransform.DOScale(TargetScale, FlightDuration).SetEase(FlightEase));
                FlightSequence.Join(CubicTransform.DOBlendableRotateBy(Vector3.right * RotationAngle * RotationSpeed, FlightDuration, RotateMode.FastBeyond360)).SetEase(RotationEase);
            });
        dragSequence.Join(CubicTransform.DOShakeRotation(_dragDuration, strength: new Vector3(2, 0, 2), vibrato: 30, randomness: 100)).SetEase(_dragEase);
    }

    private void OnPistonWorkCompleted()
    {
        if(_isCubicLeft == false)
        {
            Invoke(nameof(SuckIn), 1);
        }
    }
}
