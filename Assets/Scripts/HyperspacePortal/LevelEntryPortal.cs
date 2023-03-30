using DG.Tweening;
using System;
using UnityEngine;

public class LevelEntryPortal : HyperspacePortal
{
    public event Action ThrowingOut;

    private void Start()
    {
        TargetPositionY = CubicTransform.position.y;
        TargetScale = CubicTransform.transform.localScale;
        CubicTransform.localScale = Vector3.zero;
        Invoke(nameof(ThrowOut), 2);
    }

    private void ThrowOut()
    {
        ThrowingOut?.Invoke();
        Quaternion startRotation = CubicTransform.rotation;
        CubicTransform.position = Center.position;

        FlightSequence = DOTween.Sequence();
        FlightSequence.Append(CubicTransform.DOScale(TargetScale, FlightDuration * 0.5f));
        FlightSequence.Join(CubicTransform.DOBlendableLocalRotateBy(Vector3.forward * RotationAngle * RotationSpeed, FlightDuration * 0.8f, RotateMode.FastBeyond360)
            .SetEase(RotationEase))
            .OnComplete(() => CubicTransform.rotation = startRotation);
        FlightSequence.Join(CubicTransform.DOMoveY(TargetPositionY, FlightDuration).SetEase(FlightEase));
    }
}
