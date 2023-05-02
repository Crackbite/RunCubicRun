using System;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Cubic), typeof(CubicInputHandler))]
[RequireComponent(typeof(CubicSpeedController), typeof(SidewayMovement))]
public class CubicMovement : MonoBehaviour
{
    [SerializeField] private PistonPresser _pistonPresser;
    [SerializeField] private BlockDestroyer _blockDestroyer;
    [SerializeField] private float _leavePressTime = .8f;
    [SerializeField] private float _leavePressDistance = 5f;
    [SerializeField] private BlockStack _blockStack;
    [SerializeField] private float _fallPositionY = -10f;

    private bool _canLeavePress;
    private bool _canLineChange = true;
    private bool _canMove = true;

    private Cubic _cubic;
    private CubicInputHandler _cubicInputHandler;
    private CubicSpeedController _cubicSpeedController;
    private SidewayMovement _sidewaysMovement;

    public event Action CubicLeftPress;
    public event Action CubicOnStand;

    private void Awake()
    {
        _cubic = GetComponent<Cubic>();
        _cubicInputHandler = GetComponent<CubicInputHandler>();
        _cubicSpeedController = GetComponent<CubicSpeedController>();
        _sidewaysMovement = GetComponent<SidewayMovement>();
    }

    private void OnEnable()
    {
        _cubic.Hit += OnHit;
        _cubic.SteppedOnStand += CubicOnSteppedOnStand;

        _pistonPresser.CubicReached += WholePistonOnCubicReached;
        _blockDestroyer.LeavePressAllowed += OnLeavePressAllowed;

        _cubicInputHandler.LineChanged += OnLineChanged;
        _cubicInputHandler.PressEscaped += OnPressEscaped;

        _sidewaysMovement.LineReached += OnLineReached;
        _blockStack.BlocksEnded += OnBlockStackBlocksEnded;
    }

    private void Update()
    {
        bool isFalling = _cubic.transform.position.y <= _fallPositionY;

        if (_canMove && isFalling == false)
        {
            _cubic.transform.Translate(
                _cubicSpeedController.CurrentSpeed * Time.deltaTime * Vector3.right,
                Space.World);
        }
    }

    private void OnDisable()
    {
        _cubic.Hit -= OnHit;
        _cubic.SteppedOnStand -= CubicOnSteppedOnStand;

        _pistonPresser.CubicReached -= WholePistonOnCubicReached;
        _blockDestroyer.LeavePressAllowed -= OnLeavePressAllowed;

        _cubicInputHandler.LineChanged -= OnLineChanged;
        _cubicInputHandler.PressEscaped -= OnPressEscaped;

        _sidewaysMovement.LineReached -= OnLineReached;
        _blockStack.BlocksEnded -= OnBlockStackBlocksEnded;
    }

    public void EscapeFromPress()
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

    public void MoveToSide(Vector3 direction)
    {
        if (_canLineChange)
        {
            _canLineChange = false;
            _sidewaysMovement.Move(direction);
        }
    }

    private void CubicOnSteppedOnStand(PressStand pressStand)
    {
        if (_canLineChange == false)
        {
            return;
        }

        _canMove = false;
        _canLineChange = false;

        Vector3 standCenter = pressStand.Bounds.center;
        var nextPosition = new Vector3(standCenter.x, _cubic.transform.position.y, standCenter.z);

        _cubic.transform.DOMove(nextPosition, _cubicSpeedController.StopAtPressStandSpeed)
            .OnComplete(() => CubicOnStand?.Invoke());
    }

    private void OnBlockStackBlocksEnded()
    {
        _canMove = false;
        _canLineChange = false;
    }

    private void OnHit(Vector3 contactPoint, float obstacleHeight)
    {
        _canLineChange = false;

        if (_cubic.IsSawing)
        {
            _cubicSpeedController.SlowDown();
        }
        else
        {
            _canMove = false;
        }
    }

    private void OnLeavePressAllowed()
    {
        _canLeavePress = true;
    }

    private void OnLineChanged(Vector3 direction)
    {
        MoveToSide(direction);
    }

    private void OnLineReached()
    {
        _canLineChange = _canMove;
    }

    private void OnPressEscaped()
    {
        EscapeFromPress();
    }

    private void WholePistonOnCubicReached(Cubic cubic)
    {
        _canLeavePress = false;
    }
}