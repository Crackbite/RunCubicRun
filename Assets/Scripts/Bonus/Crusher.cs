using UnityEngine;

public class Crusher : Bonus
{
    [SerializeField] private Cubic _cubic;

    public override void Apply()
    {
        _cubic.CanDestroy = true;
    }

    public override void Cancel()
    {
        _cubic.CanDestroy = false;
    }
}