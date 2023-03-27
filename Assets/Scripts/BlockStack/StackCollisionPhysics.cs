using UnityEngine;

public class StackCollisionPhysics : MonoBehaviour
{
    [SerializeField] private Cubic _cubic;
    [SerializeField] private ColorBlockCollection _blockCollection;

    private void OnEnable()
    {
        _blockCollection.BlockAdded += OnBlockAdded;
        _cubic.Hit += OnHit;
    }

    private void OnDisable()
    {
        _blockCollection.BlockAdded -= OnBlockAdded;
        _cubic.Hit -= OnHit;
    }

    public void OnCrossbarHit(int stackPosition)
    {
        const float BlockDestroyDelay = 5f;
        const float ForceFactor = 0.1f;

        int brokenBlocksCount = _blockCollection.Blocks.Count - stackPosition;

        for (int i = 1; i <= brokenBlocksCount; i++)
        {
            _blockCollection.Blocks[0].BlockPhysics.FallOff(Vector3.left, ForceFactor);
            _blockCollection.Destroy(_blockCollection.Blocks[0], BlockDestroyDelay);
            _blockCollection.Blocks[0].BlockPhysics.CrossbarHit -= OnCrossbarHit;
        }
    }

    private void Collapse()
    {
        Vector3 fallDirection = Vector3.right;
        Vector3 trapPosition = Vector3.zero;

        if (_cubic.CollisionTrap != null)
        {
            trapPosition = _cubic.CollisionTrap.transform.position;
        }

        if (_cubic.IsSideCollision)
        {
            fallDirection = trapPosition.z > _cubic.transform.position.z
                                ? fallDirection + Vector3.forward
                                : fallDirection + Vector3.back;
        }

        foreach (ColorBlock block in _blockCollection.Blocks)
        {
            block.BlockPhysics.FallOff(fallDirection);
        }
    }

    private void OnBlockAdded(ColorBlock colorBlock)
    {
        colorBlock.BlockPhysics.CrossbarHit += OnCrossbarHit;
    }

    private void OnHit()
    {
        Collapse();
    }
}