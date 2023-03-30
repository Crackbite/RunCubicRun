using DG.Tweening;
using System;
using UnityEngine;

public class LevelEntryPortal : HyperspacePortal
{
    [SerializeField] private Ease _flightEase = Ease.InCubic;
    [SerializeField] private Ease _rotationEase = Ease.OutQuad;

    private float _targetPositionY;
    private const float Delay = 2f;
    private const float ScaleDurationMultiplier = 0.5f;
    private const float RotateDurationMultiplier = 0.8f;

    public event Action ThrowingOut;

    private void Start()
    {
        _targetPositionY = CubicTransform.position.y;
        TargetScale = CubicTransform.transform.localScale;
        CubicTransform.localScale = Vector3.zero;
        Invoke(nameof(ThrowOut), Delay);
    }

    private void ThrowOut()
    {
        ThrowingOut?.Invoke();
        Quaternion startRotation = CubicTransform.rotation;
        CubicTransform.position = Center.position;

        Sequence throwSequence = DOTween.Sequence();

        Tween scaling = CubicTransform.DOScale(TargetScale, FlightDuration * ScaleDurationMultiplier);
        Tween rotation = CubicTransform.DORotate(Vector3.forward * RotationAngle * RotationSpeed, FlightDuration * RotateDurationMultiplier, RotateMode.FastBeyond360);
        Tween flight = CubicTransform.DOMoveY(_targetPositionY, FlightDuration).SetEase(_flightEase);

        throwSequence.Append(scaling);
        throwSequence.Join(rotation)
            .SetEase(_rotationEase)
            .OnComplete(() => CubicTransform.rotation = startRotation);
        throwSequence.Join(flight);
    }
}
