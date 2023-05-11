using UnityEngine;

public class BlockStackPhysics : MonoBehaviour
{
    [SerializeField] private Cubic _cubic;
    [SerializeField] private BlockStack _blockStack;
    [SerializeField] private float _maxPushForce = 200f;
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
            _blockStack.Blocks[0].BlockPhysics.FallOff(GetCurrentPushForce(Vector3.left), ForceFactor);
            _blockStack.Destroy(_blockStack.Blocks[0], _blockDestroyDelay);
            _blockStack.Blocks[0].BlockPhysics.CrossbarHit -= OnCrossbarHit;
        }
    }

    private void Collapse(Vector3 contactPoint, float obstacleHeight)
    {
        const float ForceFactor = 1f;

        if (_blockStack.Blocks.Count == 0)
        {
            return;
        }

        Vector3 fallDirection = GetFallDirection(contactPoint, obstacleHeight);

        foreach (ColorBlock block in _blockStack.Blocks)
        {
            block.BlockPhysics.FallOff(GetCurrentPushForce(fallDirection), ForceFactor, false);
        }
    }

    private Vector3 GetCurrentPushForce(Vector3 fallDirection)
    {
        float forceMultiplier = 1f / _blockStack.Blocks.Count;

        return _maxPushForce * forceMultiplier * fallDirection;
    }

    private Vector3 GetFallDirection(Vector3 contactPoint, float obstacleHeight)
    {
        Vector3 fallDirection = contactPoint - _cubic.transform.position;
        fallDirection = new Vector3(fallDirection.x, 0f, fallDirection.z);

        if (obstacleHeight > _blockStack.Height)
        {
            return -fallDirection.normalized;
        }

        return fallDirection.normalized;
    }

    private void OnBlockAdded(ColorBlock colorBlock)
    {
        colorBlock.BlockPhysics.CrossbarHit += OnCrossbarHit;
    }

    private void OnCubicSuckingIn()
    {
        const float ForceFactor = 0.1f;

        foreach (ColorBlock block in _blockStack.Blocks)
        {
            block.BlockPhysics.FallOff(GetCurrentPushForce(Vector3.zero), ForceFactor);
        }
    }

    private void OnHit(Vector3 contactPoint, float obstacleHeight)
    {
        Collapse(contactPoint, obstacleHeight);
    }
}