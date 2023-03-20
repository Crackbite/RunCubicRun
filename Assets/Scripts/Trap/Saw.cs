using UnityEngine;

public class Saw : Trap
{
    [SerializeField] private bool _isVertical;
    [SerializeField] private VerticalSplitter _verticalSplitter;
    [SerializeField] private HorizontalSplitter _horizontalSplitter;

    public bool IsVertical => _isVertical;

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Cubic>(out Cubic cubic))
        {
            if (cubic.CanDestroy)
            {
                Break();
            }
            else
            {
                IsSideCollision = Mathf.Abs(transform.position.z - cubic.transform.position.z) >= transform.localScale.z / 2;

                if (IsVertical && IsSideCollision == true)
                    Stop();

                cubic.HitTrap(this, IsSideCollision);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Cubic>(out Cubic cubic) && cubic.IsSawing)
            cubic.SplitIntoPieces(IsVertical);
    }
}
