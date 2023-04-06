using UnityEngine;

public abstract class Saw : Trap
{
    private void OnTriggerExit(Collider collision)
    {
        if (collision.TryGetComponent(out Cubic cubic) && cubic.IsSawing)
        {
            cubic.SplitIntoPieces(this);
        }
    }
}