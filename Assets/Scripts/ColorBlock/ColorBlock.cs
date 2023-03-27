using UnityEngine;

[RequireComponent(typeof(ColorBlockRenderer), typeof(ColorBlockPhysics), typeof(ColorBlockMovement))]
public class ColorBlock : MonoBehaviour
{
    private BlocksContainer _blockContainer;
    private ColorBlockMovement _blockMovement;

    public ColorBlockPhysics BlockPhysics { get; private set; }
    public ColorBlockRenderer BlockRenderer { get; private set; }

    public bool CanFollow { get; private set; }
    public int StackPosition { get; private set; }

    private void Awake()
    {
        BlockRenderer = GetComponent<ColorBlockRenderer>();
        BlockPhysics = GetComponent<ColorBlockPhysics>();
        _blockMovement = GetComponent<ColorBlockMovement>();
    }

    public void Init(BlocksContainer blockContainer, BlockMovementCoordinator movementCoordinator)
    {
        _blockContainer = blockContainer;
        SetStackPosition();
        _blockMovement.StartFollowing(movementCoordinator);
        CanFollow = true;
    }

    public void SetStackPosition()
    {
        StackPosition = _blockContainer.GetStackPosition(this);
    }

    public void StopFollow()
    {
        CanFollow = false;
    }
}