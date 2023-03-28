using UnityEngine;
using DG.Tweening;
using System;

public class StagePortal : MonoBehaviour
{
    [SerializeField] private Transform _cubicTransform;
    [SerializeField] private Transform _center;
    [SerializeField] private float _flightDuration = 1f; 
    [SerializeField] private float _rotationAngle = 360f; 
    [SerializeField] private Ease _fallEase = Ease.Linear; 
    [SerializeField] private Ease _rotationEase = Ease.Linear;

    private Vector3 _targetScale;
    private Vector3 _startPosition;
    private Sequence _flightSequence;
    private float _targetPositionY;

    public event Action ThrownOut;

    private void Start()
    {
        _targetPositionY = _cubicTransform.position.y;
        _flightSequence = DOTween.Sequence();
        _cubicTransform.position = _center.position;
        _targetScale = _cubicTransform.transform.localScale;
        _cubicTransform.localScale = Vector3.zero;
        Invoke(nameof(Throw),2);
    }

    void Throw()
    {
        ThrownOut?.Invoke();
        Quaternion startRotation = _cubicTransform.rotation;

        _flightSequence = DOTween.Sequence();
        _flightSequence.Append(_cubicTransform.DOScale(_targetScale, _flightDuration * 0.5f));
        _flightSequence.Join(_cubicTransform.DORotate(new Vector3(0, 0, _rotationAngle), _flightDuration * 0.8f, RotateMode.FastBeyond360)).SetEase(_rotationEase);
        _flightSequence.Join(_cubicTransform.DOMoveY(_targetPositionY, _flightDuration).SetEase(_fallEase));
        _flightSequence.Play().OnComplete(() => _cubicTransform.rotation = startRotation);
    }
}

