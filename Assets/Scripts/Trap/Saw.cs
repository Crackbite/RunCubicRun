using UnityEngine;

public class Saw : Trap
{
    [SerializeField] private SplitType _splitType;

    private const float Threshold = .5f;

    public SplitType SplitType => _splitType;

    protected override void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent(out Cubic cubic) == false)
        {
            return;
        }

        if (cubic.CanDestroy)
        {
            Break();
        }
        else
        {
            IsSideCollision = Mathf.Abs(transform.position.z - cubic.transform.position.z) > Threshold;

            if (_splitType == SplitType.Vertical && IsSideCollision)
            {
                Stop();
            }

            cubic.HitTrap(this, IsSideCollision);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.TryGetComponent(out Cubic cubic) && cubic.IsSawing)
        {
            cubic.SplitIntoPieces(_splitType);
        }
    }
}