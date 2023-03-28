using DG.Tweening;
using System;
using UnityEngine;

public class LevelExitPortal : HyperspacePortal
{
    [SerializeField] private CubicMovement _cubicMovement;

    private float _dragDistance = 2f;
    private float _dragDuration = 3f;

    public event Action SuckingIn;

    private void OnEnable()
    {
        _cubicMovement.CubicLeftPress += OnCubicLeftPress;
    }

    private void OnDisable()
    {
        _cubicMovement.CubicLeftPress -= OnCubicLeftPress;
    }

    private void OnCubicLeftPress()
    {
        TargetPositionY = Center.position.y;
        Invoke(nameof(SuckIn), 3);
    }

    private void SuckIn()
    {
        SuckingIn?.Invoke();

        FlightSequence = DOTween.Sequence();
        FlightSequence.Append(CubicTransform.DOShakeRotation(2f, strength: new Vector3(0, 0, 10), vibrato: 30, randomness: 90, fadeOut: true));
        FlightSequence.Join(CubicTransform.DOMoveX(CubicTransform.position.x + _dragDistance, _dragDuration).SetEase(FlightEase))
            .OnComplete(() => CubicTransform.DOMove(Center.position, FlightDuration).SetEase(FlightEase));
        FlightSequence.Play();
    }
}
