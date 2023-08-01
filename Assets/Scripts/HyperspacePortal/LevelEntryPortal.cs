using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class LevelEntryPortal : HyperspacePortal
{
    [SerializeField] private Ease _flightEase = Ease.InCubic;
    [SerializeField] private Ease _rotationEase = Ease.OutQuad;
    [SerializeField] private float _rotationDurationFactor = .8f;
    [SerializeField] private float _scalingDurationFactor = .5f;
    [SerializeField] private GameStatusTracker _gameStatusTracker;
    [SerializeField] private ParticleSystem _landingEffect;

    private float _targetYPosition;

    public event Action ThrowingOut;
    public event Action ThrownOut;

    private void OnEnable()
    {
        _gameStatusTracker.GameStarted += OnGameStarted;
    }

    private void Start()
    {
        _targetYPosition = CubicTransform.position.y;
        TargetScale = CubicTransform.transform.localScale;
        CubicTransform.localScale = Vector3.zero;
    }

    private void OnDisable()
    {
        _gameStatusTracker.GameStarted -= OnGameStarted;
    }

    private void OnGameStarted()
    {
        StartCoroutine(ThrowOut());
    }

    private IEnumerator ThrowOut()
    {
        yield return new WaitForSeconds(Delay);

        ThrowingOut?.Invoke();

        Quaternion startRotation = CubicTransform.rotation;
        CubicTransform.position = Center.position;

        Sequence throwSequence = DOTween.Sequence();

        Tween scaling = CubicTransform.DOScale(TargetScale, FlightDuration * _scalingDurationFactor);
        Tween rotation = CubicTransform
            .DORotate(
                Vector3.forward * (RotationAngle * RotationSpeed),
                FlightDuration * _rotationDurationFactor,
                RotateMode.FastBeyond360).SetEase(_rotationEase)
            .OnComplete(() => CubicTransform.rotation = startRotation);
        Tween flight = CubicTransform.DOMoveY(_targetYPosition, FlightDuration).SetEase(_flightEase);

        throwSequence.Append(scaling);
        throwSequence.Join(rotation);
        throwSequence.Join(flight);

        throwSequence.OnComplete(() =>
        {
            ThrownOut?.Invoke();
            Vector3 effectPosition = CubicTransform.position;
            _landingEffect.transform.position = new Vector3(effectPosition.x, _landingEffect.transform.position.y, effectPosition.z);
            _landingEffect.Play();
        });
    }
}