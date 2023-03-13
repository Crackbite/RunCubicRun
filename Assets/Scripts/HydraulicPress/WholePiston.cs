using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(PressSpeedHandler))]
public class WholePiston : MonoBehaviour
{
    [SerializeField] private float _waitBeforePress = 1f;
    [SerializeField] private float _waitBeforeCubicPress = 1f;
    [SerializeField] private Vector3 _shakeStrength = new Vector3(.02f, 0f, .02f);
    [SerializeField] private PressStand _pressStand;
    [SerializeField] private BlocksContainer _blocksContainer;
    [SerializeField] private int _minBlocksToLeave = 8;

    private bool _cubicReached;
    private bool _pressed;
    private PressSpeedHandler _pressSpeedHandler;
    private float _pressStandHighestPoint;

    public event Action LeavePressAllowed;
    public event Action WorkCompleted;

    private void Start()
    {
        _pressSpeedHandler = GetComponent<PressSpeedHandler>();
        _pressStandHighestPoint = _pressStand.GetComponent<MeshRenderer>().bounds.max.y;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent(out Cubic cubic))
        {
            if (_cubicReached == false)
            {
                StartCoroutine(ShakeAndPress(cubic));
            }
            else
            {
                PressCubic(cubic);
            }
        }
        else if (collision.TryGetComponent(out ColorBlock colorBlock))
        {
            if (_pressed == false)
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
        if (_pressed == false)
        {
            return;
        }

        float speed = _cubicReached ? _pressSpeedHandler.CubicPressSpeed : _pressSpeedHandler.GetCurrentSpeed();

        Vector3 currentPosition = transform.position;
        Vector3 newPosition = currentPosition;
        newPosition.y = _pressStandHighestPoint;

        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(currentPosition, newPosition, step);

        if (Mathf.Approximately(newPosition.y, transform.position.y))
        {
            _pressed = false;
            WorkCompleted?.Invoke();
        }

    }

    private void DestroyBlock(ColorBlock colorBlock)
    {
        if (_blocksContainer.BlocksCount <= _minBlocksToLeave + 1)
        {
            LeavePressAllowed?.Invoke();
        }

        _blocksContainer.DestroyBlock(colorBlock);
    }

    private IEnumerator PressAndDestroy(ColorBlock colorBlock)
    {
        yield return new WaitForSeconds(_waitBeforePress);
        _pressed = true;
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

        _cubicReached = true;
        _pressed = false;

        Tweener tweener = cubic.transform.DOShakePosition(
            _waitBeforeCubicPress * ShakeDurationMultiplier,
            _shakeStrength,
            randomnessMode: ShakeRandomnessMode.Harmonic);

        yield return new WaitForSeconds(_waitBeforeCubicPress);

        _pressed = true;
        PressCubic(cubic);

        tweener.Kill();
    }
}