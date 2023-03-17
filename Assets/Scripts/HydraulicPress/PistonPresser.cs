using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(PistonMover), typeof(BlockDestroyer))]
public class PistonPresser : MonoBehaviour
{
    [SerializeField] private float _pressDelay = 1f;
    [SerializeField] private float _cubicPressDelay = 1f;
    [SerializeField] private Vector3 _cubicShakeStrength = new Vector3(.02f, 0f, .02f);
    [SerializeField] private CubicMovement _cubicMovement;

    private BlockDestroyer _blockDestroyer;
    private bool _isCubicCollisionDisabled;
    private bool _isCubicReached;
    private PistonMover _pistonMover;

    public event Action CubicReached;

    private void OnEnable()
    {
        _cubicMovement.CubicLeftPress += OnCubicLeftPress;
    }

    private void Start()
    {
        _pistonMover = GetComponent<PistonMover>();
        _blockDestroyer = GetComponent<BlockDestroyer>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent(out Cubic cubic) && _isCubicCollisionDisabled == false)
        {
            if (_isCubicReached == false)
            {
                CubicReached?.Invoke();
                StartCoroutine(ShakeAndPress(cubic));
            }
            else
            {
                PressCubic(cubic);
            }
        }
        else if (collision.TryGetComponent(out ColorBlock colorBlock))
        {
            if (_pistonMover.IsWorking == false)
            {
                StartCoroutine(PressAndDestroy(colorBlock));
            }
            else
            {
                _blockDestroyer.DestroyBlock(colorBlock);
            }
        }
    }

    private void OnDisable()
    {
        _cubicMovement.CubicLeftPress -= OnCubicLeftPress;
    }

    private void OnCubicLeftPress()
    {
        _isCubicCollisionDisabled = true;
    }

    private IEnumerator PressAndDestroy(ColorBlock colorBlock)
    {
        yield return new WaitForSeconds(_pressDelay);

        StartPress();
        _blockDestroyer.DestroyBlock(colorBlock);
    }

    private void PressCubic(Cubic cubic)
    {
        float extentsY = cubic.Bounds.extents.y;
        cubic.transform.position = transform.TransformPoint(Vector3.down * extentsY);
    }

    private IEnumerator ShakeAndPress(Cubic cubic)
    {
        const float ShakeDurationMultiplier = 2f;

        _isCubicReached = true;
        _pistonMover.TurnOff();

        Tweener tweener = cubic.transform.DOShakePosition(
            _cubicPressDelay * ShakeDurationMultiplier,
            _cubicShakeStrength,
            randomnessMode: ShakeRandomnessMode.Harmonic);

        yield return new WaitForSeconds(_cubicPressDelay);

        _pistonMover.TurnOn();
        PressCubic(cubic);

        tweener.Kill();
    }

    private void StartPress()
    {
        _pistonMover.Init();
        _pistonMover.TurnOn();
    }
}