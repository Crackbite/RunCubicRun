using System;
using UnityEngine;

public class BlockStackPhysics : MonoBehaviour
{
    [SerializeField] private Cubic _cubic;
    [SerializeField] private BlockStack _blockStack;
    [SerializeField] private float _frictionCoefficient = 10f;
    [SerializeField] private float _blockDestroyDelay = 5f;
    [SerializeField] private LevelExitPortal _levelExitPortal;

    private void OnEnable()
    {
        _blockStack.BlockAdded += OnBlockAdded;
        _cubic.Hit += OnHit;
        _levelExitPortal.SuckingIn += OnCubicSuckingIn;
    }

    private void OnDisable()
    {
        _blockStack.BlockAdded -= OnBlockAdded;
        _cubic.Hit -= OnHit;
        _levelExitPortal.SuckingIn -= OnCubicSuckingIn;

    }


    public void OnCrossbarHit(int stackPosition)
    {
        const float ForceFactor = 0.1f;

        int brokenBlocksCount = _blockStack.Blocks.Count - stackPosition;

        for (int i = 1; i <= brokenBlocksCount; i++)
        {
            _blockStack.Blocks[0].BlockPhysics.FallOff(Vector3.left, _frictionCoefficient, ForceFactor);
            _blockStack.Destroy(_blockStack.Blocks[0], _blockDestroyDelay);
            _blockStack.Blocks[0].BlockPhysics.CrossbarHit -= OnCrossbarHit;
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

        foreach (ColorBlock block in _blockStack.Blocks)
        {
            block.BlockPhysics.FallOff(fallDirection, _frictionCoefficient);
        }
    }

    private void OnCubicSuckingIn()
    {
        const float ForceFactor = 0.1f;

        foreach (ColorBlock block in _blockStack.Blocks)
        {
            block.BlockPhysics.FallOff(Vector3.zero, _frictionCoefficient, ForceFactor);
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