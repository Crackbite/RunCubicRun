using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class LevelExitPortal : HyperspacePortal
{
    [SerializeField] private CubicMovement _cubicMovement;
    [SerializeField] private PistonMover _pistonMover;
    [SerializeField] private float _dragDistance = 5f;
    [SerializeField] private float _dragDuration = 3f;
    [SerializeField] private float _randomness = 100f;
    [SerializeField] private int _vibration = 30;
    [SerializeField] private Vector3 _strength = new Vector3(2f, 0f, 2f);

    private bool _isCubicLeft;

    private Coroutine _suckInCoroutine;

    public event Action SuckedIn;
    public event Action SuckingIn;
    public event Action Shaked;

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
        StartSuckIn();
    }

    private void OnPistonWorkCompleted()
    {
        if (_isCubicLeft == false)
        {
            StartSuckIn();
        }
    }

    private void StartSuckIn()
    {
        if (_suckInCoroutine != null)
        {
            StopCoroutine(_suckInCoroutine);
        }

        _suckInCoroutine = StartCoroutine(SuckIn());
    }

    private IEnumerator SuckIn()
    {
        yield return new WaitForSeconds(Delay);

        SuckingIn?.Invoke();
        
        TargetScale = Vector3.zero;
        Sequence dragSequence = DOTween.Sequence();

        Tween dragByX = CubicTransform.DOMoveX(CubicTransform.position.x + _dragDistance, _dragDuration)
            .SetEase(Ease.Linear);
        Tween shaking = CubicTransform.DOShakeRotation(
            _dragDuration,
            strength: _strength,
            vibrato: _vibration,
            randomness: _randomness).SetEase(Ease.Linear)
            .OnComplete(() => 
            {
                Cubic.SoundSystem.Play(SoundEvent.PortalEntry);
                Shaked?.Invoke(); 
            });
        Tween flight = CubicTransform.DOMove(Center.position, FlightDuration).SetEase(Ease.Linear);
        Tween scaling = CubicTransform.DOScale(TargetScale, FlightDuration).SetEase(Ease.Linear);
        Tween rotation = CubicTransform.DORotate(
            Vector3.right * (RotationAngle * RotationSpeed),
            FlightDuration,
            RotateMode.FastBeyond360).SetEase(Ease.Linear);

        dragSequence.Append(dragByX);
        dragSequence.Join(shaking);
        dragSequence.Append(flight);
        dragSequence.Join(scaling);
        dragSequence.Join(rotation);

        dragSequence.OnComplete(() => { SuckedIn?.Invoke(); });
    }
}