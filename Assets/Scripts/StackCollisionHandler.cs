using System.Collections.Generic;
using UnityEngine;

public class StackCollisionHandler : MonoBehaviour
{
    [SerializeField] private Cubic _cubic;
    [SerializeField] private BlockStacker _blockStacker;

    private readonly List<ColorBlock> _blocks = new();

    private void OnEnable()
    {
        _blockStacker.ColorBlockAdded += OnColorBlockAdded;
        _cubic.Hit += OnHit;
    }

    private void OnDisable()
    {
        _blockStacker.ColorBlockAdded -= OnColorBlockAdded;
        _cubic.Hit -= OnHit;
    }

    public void OnCrossbarHit(int stackPosition)
    {
        const float ForceFactor = 0.1f;

        int brokenBlocksCount = _blocks.Count - stackPosition;

        for (int i = 1; i <= brokenBlocksCount; i++)
        {
            _blocks[0].BlockPhysics.FallOff(Vector3.left, ForceFactor);
            _blocks.Remove(_blocks[0]);
            _blocks[0].BlockPhysics.CrossbarHit -= OnCrossbarHit;
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

        foreach (ColorBlock block in _blocks)
        {
            block.BlockPhysics.FallOff(fallDirection);
        }
    }

    private void OnHit()
    {
        Collapse();
    }

    private void OnColorBlockAdded(ColorBlock colorBlock)
    {
        _blocks.Add(colorBlock);

        colorBlock.BlockPhysics.CrossbarHit += OnCrossbarHit;
    }
}
