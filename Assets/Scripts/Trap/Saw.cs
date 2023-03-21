using UnityEngine;

public class Saw : Trap
{
    [SerializeField] private bool _isVertical;

    public bool IsVertical => _isVertical;

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
            IsSideCollision = Mathf.Abs(transform.position.z - cubic.transform.position.z)
                              >= transform.localScale.z / 2f;

            if (IsVertical && IsSideCollision)
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
            cubic.SplitIntoPieces(IsVertical);
        }
    }
}