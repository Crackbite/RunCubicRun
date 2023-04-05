using UnityEngine;

public class Saw : Trap
{
    [SerializeField] private SplitType _splitType;

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
            if (_splitType == SplitType.Vertical)
            {
                if (Mathf.Abs(transform.position.z - cubic.transform.position.z) > Threshold)
                {
                    IsSideCollision = true;
                    Stop();
                }
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