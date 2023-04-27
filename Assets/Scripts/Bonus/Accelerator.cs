using UnityEngine;

public class Accelerator : Bonus
{
    [SerializeField] private float _speedMultiplier = 2f;
    [SerializeField] private CubicSpeedController _cubicSpeedController;

    private float _initialSpeedMultiplier;

    private void Start()
    {
        if (_cubicSpeedController == null)
        {
            _cubicSpeedController = FindObjectOfType<CubicSpeedController>();
        }
    }

    public override void Apply()
    {
        _initialSpeedMultiplier = _cubicSpeedController.SpeedMultiplier;
        _cubicSpeedController.SetSpeedMultiplier(_speedMultiplier);
    }

    public override void Cancel()
    {
        _cubicSpeedController.SetSpeedMultiplier(_initialSpeedMultiplier);
    }
}