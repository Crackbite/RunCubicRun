using UnityEngine;

[RequireComponent(typeof(PressSpeedHandler), typeof(WholePiston))]
public class PressSpeedReducer : MonoBehaviour
{
    [SerializeField] private float _decreaseRate = .2f;
    [SerializeField] private float _increaseSpeed = 1f;
    [SerializeField] private float _timeThreshold = .1f;
    [SerializeField] private float _minSpeed = .1f;

    private bool _canReduceSpeed;
    private PressSpeedHandler _pressSpeedHandler;
    private float _speed;
    private float _timeSinceLastClick;
    private WholePiston _wholePiston;

    private void Awake()
    {
        _pressSpeedHandler = GetComponent<PressSpeedHandler>();
        _wholePiston = GetComponent<WholePiston>();
    }

    private void OnEnable()
    {
        _wholePiston.LeavePressAllowed += OnLeavePressAllowed;
        _wholePiston.WorkCompleted += OnWorkCompleted;
    }

    private void Update()
    {
        if (_canReduceSpeed == false)
        {
            return;
        }

        if (_timeSinceLastClick > _timeThreshold)
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
        _wholePiston.LeavePressAllowed -= OnLeavePressAllowed;
        _wholePiston.WorkCompleted -= OnWorkCompleted;
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

    private void OnLeavePressAllowed()
    {
        _speed = _pressSpeedHandler.GetCurrentSpeed();
        _canReduceSpeed = true;
        _wholePiston.LeavePressAllowed -= OnLeavePressAllowed;
    }

    private void OnWorkCompleted()
    {
        _canReduceSpeed = false;
    }
}