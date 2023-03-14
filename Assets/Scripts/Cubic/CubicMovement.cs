using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Cubic))]
public class CubicMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _changeLineSpeed = .1f;
    [SerializeField] private float _shiftPerMove = 1.3f;
    [SerializeField] private float _stopAtPressStandSpeed = 1f;
    [SerializeField] private WholePiston _wholePiston;
    [SerializeField] private float _leavePressTime = .8f;
    [SerializeField] private float _leavePressDistance = 5f;

    private bool _canLeavePress;
    private bool _canLineChange = true;
    private bool _canMove = true;

    private Cubic _cubic;

    private float _maxPositionZ;
    private float _minPositionZ;

    public event Action CubicLeftPress;
    public event Action CubicOnStand;

    private void Awake()
    {
        _cubic = GetComponent<Cubic>();

        Vector3 position = _cubic.transform.position;
        _minPositionZ = position.z - _shiftPerMove;
        _maxPositionZ = position.z + _shiftPerMove;
    }

    private void OnEnable()
    {
        _cubic.SteppedOnStand += CubicOnSteppedOnStand;
        _wholePiston.LeavePressAllowed += OnLeavePressAllowed;
        _wholePiston.CubicReached += WholePistonOnCubicReached;
    }

    private void Update()
    {
        if (_canMove)
        {
            _cubic.transform.Translate(_moveSpeed * Time.deltaTime * Vector3.right);
        }
    }

    private void OnDisable()
    {
        _cubic.SteppedOnStand -= CubicOnSteppedOnStand;
        _wholePiston.LeavePressAllowed -= OnLeavePressAllowed;
        _wholePiston.CubicReached -= WholePistonOnCubicReached;
    }

    public void MoveForward()
    {
        if (_canLeavePress == false)
        {
            return;
        }

        float newPositionX = _cubic.transform.position.x + _leavePressDistance;
        _cubic.transform.DOMoveX(newPositionX, _leavePressTime).SetEase(Ease.OutQuart);

        _canLeavePress = false;
        _wholePiston.LeavePressAllowed -= OnLeavePressAllowed;

        CubicLeftPress?.Invoke();
    }

    public void MoveLeft()
    {
        float positionZ = _cubic.transform.position.z;

        if (_canLineChange && positionZ < _maxPositionZ)
        {
            StartCoroutine(MoveToPositionZ(positionZ + _shiftPerMove));
        }
    }

    public void MoveRight()
    {
        float positionZ = _cubic.transform.position.z;

        if (_canLineChange && positionZ > _minPositionZ)
        {
            StartCoroutine(MoveToPositionZ(positionZ - _shiftPerMove));
        }
    }

    private void CubicOnSteppedOnStand(PressStand pressStand)
    {
        _canMove = false;
        _canLineChange = false;

        Vector3 standCenter = pressStand.GetComponent<Collider>().bounds.center;
        var nextPosition = new Vector3(standCenter.x, _cubic.transform.position.y, standCenter.z);

        _cubic.transform.DOMove(nextPosition, _stopAtPressStandSpeed).OnComplete(() => CubicOnStand?.Invoke());
    }

    private IEnumerator MoveToPositionZ(float positionZ)
    {
        _canLineChange = false;
        yield return _cubic.transform.DOMoveZ(positionZ, _changeLineSpeed).WaitForCompletion();
        _canLineChange = _canMove;
    }

    private void OnLeavePressAllowed()
    {
        _canLeavePress = true;
    }

    private void WholePistonOnCubicReached()
    {
        _canLeavePress = false;
    }
}