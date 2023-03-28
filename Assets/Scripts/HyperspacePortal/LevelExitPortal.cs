using DG.Tweening;
using System;
using UnityEngine;

public class LevelExitPortal : HyperspacePortal
{
    [SerializeField] private CubicMovement _cubicMovement;
    [SerializeField] private Ease _dragEase = Ease.InCubic;
    [SerializeField] private PistonMover _pistonMover;
    [SerializeField] private PressStand _pressStand;


    private float _dragDistance = 3f;
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
        Debug.Log("Left");
        _isCubicLeft = true;
        TargetPositionY = Center.position.y;
        TargetScale = Vector3.zero;
        Invoke(nameof(SuckIn), 3);
    }

    private void SuckIn()
    {
        SuckingIn?.Invoke();
        Sequence dragSequence = DOTween.Sequence();

        dragSequence.Append(CubicTransform.DOBlendableMoveBy(new Vector3(_dragDistance, 0, 0), _dragDuration).SetEase(_dragEase))
            .OnComplete(() =>
            {
                FlightSequence = DOTween.Sequence();
                FlightSequence.Append(CubicTransform.DOMove(Center.position, FlightDuration).SetEase(FlightEase)).SetSpeedBased(true);
                FlightSequence.Join(CubicTransform.DOScale(TargetScale, FlightDuration).SetDelay(FlightDuration * 0.2f));
            });
        dragSequence.Join(CubicTransform.DOShakeRotation(_dragDuration, strength: new Vector3(0, 0, 10), vibrato: 70, randomness: 100));
    }

    private void OnPistonWorkCompleted()
    {
        Debug.Log("WorkCompleted");
        if(_isCubicLeft == false)
        {
            TargetPositionY = Center.position.y;
            TargetScale = Vector3.zero;
            CubicTransform.localScale = new Vector3(CubicTransform.localScale.x, 0.1f, CubicTransform.localScale.z);
            CubicTransform.position = new Vector3(CubicTransform.position.x, _pressStand.Bounds.max.y + CubicTransform.localScale.y, CubicTransform.position.z);
            Invoke(nameof(SuckIn), 3);
        }
    }
}
