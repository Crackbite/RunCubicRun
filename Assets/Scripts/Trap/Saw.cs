using UnityEngine;

public abstract class Saw : Trap
{
    private bool _canSplit = true;

    private void OnTriggerExit(Collider collision)
    {
        if (collision.TryGetComponent(out Cubic cubic) == false || cubic.CanDestroy)
        {
            return;
        }

        if (this is not VerticalSaw && _canSplit)
        {
            cubic.SplitIntoPieces();
            _canSplit = false;
        }
    }
}