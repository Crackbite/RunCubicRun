using System;
using System.Linq;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Cubic), typeof(CubicInputHandler), typeof(CubicSpeedController))]
public class CubicMovement : MonoBehaviour
{
    [SerializeField] private const float ShiftPerMove = 1.3f;
    [SerializeField] private PistonPresser _pistonPresser;
    [SerializeField] private BlockDestroyer _blockDestroyer;
    [SerializeField] private float _leavePressTime = .8f;
    [SerializeField] private float _leavePressDistance = 5f;
    [SerializeField] private BlocksContainer _blocksContainer;
    [SerializeField] private float _fallPositionY = -10f;

    private bool _canLeavePress;
    private bool _canLineChange = true;
    private bool _canMove = true;
    private float _maxPositionZ;
    private float _minPositionZ;

    private Cubic _cubic;
    private CubicInputHandler _cubicInputHandler;
    private CubicSpeedController _cubicSpeedController;

    public event Action CubicLeftPress;
    public event Action CubicOnStand;

    private void Awake()
    { 
        _cubic = GetComponent<Cubic>();
        _cubicInputHandler = GetComponent<CubicInputHandler>();
        _cubicSpeedController = GetComponent<CubicSpeedController>();

        Vector3 position = _cubic.transform.position;
        _minPositionZ = position.z - ShiftPerMove;
        _maxPositionZ = position.z + ShiftPerMove;
    }

    private void OnEnable()
    {
        _cubic.Hit += OnHit;
        _cubic.SteppedOnStand += CubicOnSteppedOnStand;

        _pistonPresser.CubicReached += WholePistonOnCubicReached;
        _blockDestroyer.LeavePressAllowed += OnLeavePressAllowed;

        _cubicInputHandler.LineChanged += OnLineChanged;
        _cubicInputHandler.PressEscaped += OnPressEscaped;
    }

    private void Update()
    {
        bool isFalling = _cubic.transform.position.y <= _fallPositionY;

        if (_canMove && isFalling == false)
        {
            _cubic.transform.Translate(_cubicSpeedController.CurrentSpeed * Time.deltaTime * Vector3.right);
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
        const int MaxAmount = 5;

        Transform cubicTransform = _cubic.transform;
        Vector3 cubicPosition = cubicTransform.position;

        bool canMoveLeft = direction.z > 0 && cubicPosition.z < _maxPositionZ;
        bool canMoveRight = direction.z < 0 && cubicPosition.z > _minPositionZ;

        var area = new Vector3(0, cubicTransform.localScale.y, 0);
        var colliders = new Collider[MaxAmount];
        Physics.OverlapBoxNonAlloc(cubicPosition, area, colliders, Quaternion.identity);

        if ((canMoveLeft || canMoveRight) && _canLineChange)
        {
            if (colliders.Any(currentCollider => currentCollider != null && currentCollider.TryGetComponent(out Road _)) == false)
            {
                return;
            }

            ChangeLine(direction, cubicPosition.z);
        }
    }

    private void ChangeLine(Vector3 direction, float cubicPositionZ)
    {
        Tweener tween = null;
        float duration = _cubicSpeedController.GetLineChangeDuration(ShiftPerMove);
        cubicPositionZ += ShiftPerMove * direction.z;
        _canLineChange = false;
        tween = _cubic.transform.DOMoveZ(cubicPositionZ, duration).OnUpdate(() =>
        {
            if (_canMove == false)
            {
                tween.Kill();
            }
        }).OnComplete(()=> _canLineChange = _canMove);
    }


    private void CubicOnSteppedOnStand(PressStand pressStand)
    {
        if (_blocksContainer.BlocksCount < 1)
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

    private void OnHit()
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

    private void OnPressEscaped()
    {
        EscapeFromPress();
    }

    private void WholePistonOnCubicReached()
    {
        _canLeavePress = false;
    }
}