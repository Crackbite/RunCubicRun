using UnityEngine;

public class Accelerator : Bonus
{
    [SerializeField] private float _speedMultiplier = 2f;
    [SerializeField] private CubicSpeedController _cubicSpeedController;

    private float _initialSpeedMultiplier;

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