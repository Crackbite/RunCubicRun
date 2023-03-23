using UnityEngine;

[RequireComponent(typeof(ColorBlockRenderer), typeof(ColorBlockPhysics), typeof(ColorBlockMovement))]
public class ColorBlock : MonoBehaviour
{
    private BlocksContainer _blockContainer;
    private ColorBlockRenderer _blockRenderer;
    private ColorBlockPhysics _blockPhysics;
    private ColorBlockMovement _blockMovement;
    private int _stackPosition;

    public ColorBlockRenderer BlockRenderer => _blockRenderer;
    public ColorBlockPhysics BlockPhysics => _blockPhysics;

    public bool CanFollow { get; private set; }
    public int StackPosition => _stackPosition;

    private void Awake()
    {
        _blockRenderer = GetComponent<ColorBlockRenderer>();
        _blockPhysics = GetComponent<ColorBlockPhysics>();
        _blockMovement = GetComponent<ColorBlockMovement>();
    }

    public void Init(BlocksContainer blockContainer, FollowController followController)
    {
        _blockContainer = blockContainer;
        _stackPosition = _blockContainer.GetStackPosition(this);
        _blockMovement.StartFollowing(followController);
        CanFollow = true;
    }

    public void SetStackPosition()
    {
        _stackPosition = _blockContainer.GetStackPosition(this);
    }

    public void StopFollow()
    {
        CanFollow = false;
    }
}