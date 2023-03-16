using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(PressSpeedHandler))]
public class WholePiston : MonoBehaviour
{
    [SerializeField] private float _delayBeforePress = 1f;
    [SerializeField] private float _delayBeforeCubicPress = 1f;
    [SerializeField] private Vector3 _cubicShakeStrength = new Vector3(.02f, 0f, .02f);
    [SerializeField] private PressStand _pressStand;
    [SerializeField] private BlocksContainer _blocksContainer;
    [SerializeField] private int _minBlocksToLeave = 8;
    [SerializeField] private CubicMovement _cubicMovement;

    private bool _isCubicCollisionDisabled;
    private bool _isCubicReached;
    private bool _isPressed;
    private PressSpeedHandler _pressSpeedHandler;
    private float _pressStandTopYPosition;

    public event Action CubicReached;
    public event Action LeavePressAllowed;
    public event Action WorkCompleted;

    private void OnEnable()
    {
        _cubicMovement.CubicLeftPress += OnCubicLeftPress;
    }

    private void Start()
    {
        _pressSpeedHandler = GetComponent<PressSpeedHandler>();
        _pressStandTopYPosition = _pressStand.GetComponent<MeshRenderer>().bounds.max.y;
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
            if (_isPressed == false)
            {
                StartCoroutine(PressAndDestroy(colorBlock));
            }
            else
            {
                DestroyBlock(colorBlock);
            }
        }
    }

    private void Update()
    {
        if (_isPressed == false)
        {
            return;
        }

        float speed = _isCubicReached ? _pressSpeedHandler.CubicPressSpeed : _pressSpeedHandler.GetCurrentSpeed();

        Vector3 currentPosition = transform.position;
        Vector3 newPosition = currentPosition;
        newPosition.y = _pressStandTopYPosition;

        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(currentPosition, newPosition, step);

        if (Mathf.Approximately(newPosition.y, transform.position.y))
        {
            _isPressed = false;
            WorkCompleted?.Invoke();
        }
    }

    private void OnDisable()
    {
        _cubicMovement.CubicLeftPress -= OnCubicLeftPress;
    }

    private void DestroyBlock(ColorBlock colorBlock)
    {
        if (_blocksContainer.BlocksCount <= _minBlocksToLeave + 1)
        {
            LeavePressAllowed?.Invoke();
        }

        _blocksContainer.DestroyBlock(colorBlock);
    }

    private void OnCubicLeftPress()
    {
        _isCubicCollisionDisabled = true;
    }

    private IEnumerator PressAndDestroy(ColorBlock colorBlock)
    {
        yield return new WaitForSeconds(_delayBeforePress);

        StartPress();
        DestroyBlock(colorBlock);
    }

    private void PressCubic(Cubic cubic)
    {
        const float ScaleReductionFactor = 10f;
        const float PivotCorrectionFactor = 2f;
        const float MinScaleY = .01f;

        Vector3 scale = cubic.transform.localScale;
        float initScaleY = scale.y;

        Vector3 newScale = scale - (Vector3.up * (_pressSpeedHandler.CubicPressSpeed / ScaleReductionFactor));
        newScale.y = Mathf.Max(newScale.y, MinScaleY);
        cubic.transform.localScale = newScale;

        scale = cubic.transform.localScale;
        Vector3 position = cubic.transform.position;

        float localScaleY = (initScaleY - scale.y) / PivotCorrectionFactor;
        Vector3 newPosition = position - (Vector3.up * localScaleY);
        newPosition.y = Mathf.Max(newPosition.y, 0);
        cubic.transform.position = newPosition;
    }

    private IEnumerator ShakeAndPress(Cubic cubic)
    {
        const float ShakeDurationMultiplier = 2f;

        _isCubicReached = true;
        _isPressed = false;

        Tweener tweener = cubic.transform.DOShakePosition(
            _delayBeforeCubicPress * ShakeDurationMultiplier,
            _cubicShakeStrength,
            randomnessMode: ShakeRandomnessMode.Harmonic);

        yield return new WaitForSeconds(_delayBeforeCubicPress);

        _isPressed = true;
        PressCubic(cubic);

        tweener.Kill();
    }

    private void StartPress()
    {
        _pressSpeedHandler.Init();
        _isPressed = true;
    }
}