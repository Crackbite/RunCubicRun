using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Cubic), typeof(FreeSidewayChecker))]
public class CubicMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _changeLineSpeed = .1f;
    [SerializeField] private float _shiftPerMove = 1.3f;
    [SerializeField] private float _stopAtPressStandSpeed = 1f;
    [SerializeField] private PistonPresser _pistonPresser;
    [SerializeField] private BlockDestroyer _blockDestroyer;
    [SerializeField] private float _leavePressTime = .8f;
    [SerializeField] private float _leavePressDistance = 5f;
    [SerializeField] private BlocksContainer _blocksContainer;
    [SerializeField] private AnimationCurve _stopCurve;

    private bool _canLeavePress;
    private bool _canLineChange = true;
    private bool _canMove = true;
    private bool _isFall;
    private float _maxPositionZ;
    private float _minPositionZ;
    private float _fallPositionY = -10;
    private Cubic _cubic;
    private FreeSidewayChecker _sidewayChacker;

    public event Action CubicLeftPress;
    public event Action CubicOnStand;

    private void Awake()
    {
        _cubic = GetComponent<Cubic>();
        _sidewayChacker = GetComponent<FreeSidewayChecker>();

        Vector3 position = _cubic.transform.position;
        _minPositionZ = position.z - _shiftPerMove;
        _maxPositionZ = position.z + _shiftPerMove;
    }

    private void OnEnable()
    {
        _cubic.SteppedOnStand += CubicOnSteppedOnStand;
        _blockDestroyer.LeavePressAllowed += OnLeavePressAllowed;
        _pistonPresser.CubicReached += WholePistonOnCubicReached;
        _cubic.Hit += OnHit;
    }

    private void Update()
    {
        _isFall = _cubic.transform.position.y <= _fallPositionY;

        if (_canMove && _isFall == false)
        {
            _cubic.transform.Translate(_moveSpeed * Time.deltaTime * Vector3.right);
        }
    }

    private void OnDisable()
    {
        _cubic.SteppedOnStand -= CubicOnSteppedOnStand;
        _blockDestroyer.LeavePressAllowed -= OnLeavePressAllowed;
        _pistonPresser.CubicReached -= WholePistonOnCubicReached;
        _cubic.Hit -= OnHit;
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
        _blockDestroyer.LeavePressAllowed -= OnLeavePressAllowed;

        CubicLeftPress?.Invoke();
    }

    public void MoveLeft()
    {
        Vector3 direction = Vector3.forward;
        MoveToSide(direction);
    }

    public void MoveRight()
    {
        Vector3 direction = Vector3.back;
        MoveToSide(direction);
    }

    public void MoveToSide(Vector3 direction)
    {
        Collider[] colliders = Physics.OverlapSphere(_cubic.transform.position, _cubic.transform.localScale.y);
        float currentShift = _shiftPerMove;
        float currentPositionZ = _cubic.transform.position.z;
        float targetPositionZ;
        bool canMoveLeft;
        bool canMoveRight;

        currentShift = _sidewayChacker.Check(_cubic.transform, currentShift, direction);
        targetPositionZ = currentPositionZ + currentShift * direction.z;
        canMoveLeft = direction.z > 0 && currentPositionZ < _maxPositionZ;
        canMoveRight = direction.z < 0 && currentPositionZ > _minPositionZ;

        if ((canMoveLeft || canMoveRight) && _canLineChange)
        {
            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent(out Road _))
                {
                    _canLineChange = false;
                    _cubic.transform.DOMoveZ(targetPositionZ, _changeLineSpeed).OnComplete(() => _canLineChange = _canMove);
                    break;
                }
            }
        }
    }

    private void OnHit()
    {
        _canLineChange = false;

        if (_cubic.IsSawing)
        {
            StartCoroutine(StopSlowly());
        }
        else
        {
            _canMove = false;
        }
    }

    private void CubicOnSteppedOnStand(PressStand pressStand)
    {
        if (_blocksContainer.BlocksCount < 1)
        {
            return;
        }

        _canMove = false;
        _canLineChange = false;

        Vector3 standCenter = pressStand.GetComponent<Collider>().bounds.center;
        var nextPosition = new Vector3(standCenter.x, _cubic.transform.position.y, standCenter.z);

        _cubic.transform.DOMove(nextPosition, _stopAtPressStandSpeed).OnComplete(() => CubicOnStand?.Invoke());
    }

    private void OnLeavePressAllowed()
    {
        _canLeavePress = true;
    }

    private void WholePistonOnCubicReached()
    {
        _canLeavePress = false;
    }

    private IEnumerator StopSlowly()
    {
        float runningTime = 0;
        float stopDuration;
        float currentSpeed;

        stopDuration = _stopCurve.keys[_stopCurve.length - 1].time;

        while (runningTime <= stopDuration)
        {
            currentSpeed = _moveSpeed * _stopCurve.Evaluate(runningTime);
            runningTime += Time.deltaTime;
            yield return null;
        }
    }
}