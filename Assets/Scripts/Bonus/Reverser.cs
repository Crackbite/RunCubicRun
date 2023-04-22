using UnityEngine;

public class Reverser : Bonus
{
    [SerializeField] private SidewayMovement _sidewayMovement;

    public override void Apply()
    {
        _sidewayMovement.IsInvertControl = true;
    }

    public override void Cancel()
    {
        _sidewayMovement.IsInvertControl = false;
    }
}