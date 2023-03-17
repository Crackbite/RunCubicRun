using UnityEngine;

[RequireComponent(typeof(PressSpeedHandler), typeof(PistonMover), typeof(BlockDestroyer))]
public class PressSpeedReducer : MonoBehaviour
{
    [SerializeField] private float _decreaseRate = .2f;
    [SerializeField] private float _increaseSpeed = 1f;
    [SerializeField] private float _clickTimeThreshold = .1f;
    [SerializeField] private float _minSpeed = .1f;
    [SerializeField] private CubicMovement _cubicMovement;

    private BlockDestroyer _blockDestroyer;
    private bool _canReduceSpeed;
    private PistonMover _pistonMover;
    private PressSpeedHandler _pressSpeedHandler;
    private float _speed;
    private float _timeSinceLastClick;

    private void Awake()
    {
        _pressSpeedHandler = GetComponent<PressSpeedHandler>();
        _blockDestroyer = GetComponent<BlockDestroyer>();
        _pistonMover = GetComponent<PistonMover>();
    }

    private void OnEnable()
    {
        _blockDestroyer.LeavePressAllowed += OnLeavePressAllowed;
        _pistonMover.WorkCompleted += DisableReduceSpeed;
        _cubicMovement.CubicLeftPress += DisableReduceSpeed;
    }

    private void Update()
    {
        if (_canReduceSpeed == false)
        {
            return;
        }

        if (_timeSinceLastClick > _clickTimeThreshold)
        {
            _speed += _increaseSpeed * Time.deltaTime;
        }

        float currentSpeed = _pressSpeedHandler.PureSpeed;
        _speed = Mathf.Clamp(_speed, _minSpeed, currentSpeed);
        _timeSinceLastClick += Time.deltaTime;

        _pressSpeedHandler.SpeedReduceRate = currentSpeed - _speed;
    }

    private void OnDisable()
    {
        _blockDestroyer.LeavePressAllowed -= OnLeavePressAllowed;
        _pistonMover.WorkCompleted -= DisableReduceSpeed;
        _cubicMovement.CubicLeftPress -= DisableReduceSpeed;
    }

    public void ReduceSpeed()
    {
        if (_canReduceSpeed == false)
        {
            return;
        }

        _timeSinceLastClick = 0f;
        _speed -= _decreaseRate;
    }

    private void DisableReduceSpeed()
    {
        _canReduceSpeed = false;
        _pressSpeedHandler.SpeedReduceRate = 0f;
    }

    private void OnLeavePressAllowed()
    {
        _speed = _pressSpeedHandler.PureSpeed;
        _canReduceSpeed = true;
        _blockDestroyer.LeavePressAllowed -= OnLeavePressAllowed;
    }
}