using UnityEngine;

public class Reverser : Bonus
{
    [SerializeField] private SidewayMovement _sidewayMovement;

    private void Start()
    {
        if (_sidewayMovement == null)
        {
            _sidewayMovement = FindObjectOfType<SidewayMovement>();
        }
    }

    public override void Apply()
    {
        _sidewayMovement.IsInvertControl = true;
    }

    public override void Cancel()
    {
        _sidewayMovement.IsInvertControl = false;
    }
}