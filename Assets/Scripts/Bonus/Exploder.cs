using UnityEngine;

public class Exploder : Bonus
{
    [SerializeField] private BlockStack _blockStack;
    [SerializeField] private int _blocksPercentage = 50;
    [SerializeField] private float _initialForce = .3f;
    [SerializeField] private float _additionalForce = .1f;
    [SerializeField] private float _maxPushForce = 200f;
    [SerializeField] private float _blockDestroyDelay = 5f;

    public override void Apply()
    {
        if (_blockStack.Blocks.Count == 0)
        {
            return;
        }

        int brokenBlocksCount = (int)(_blockStack.Blocks.Count * (_blocksPercentage / 100f));

        for (int i = 1; i <= brokenBlocksCount; i++)
        {
            Vector3 fallDirection = Random.onUnitSphere + Vector3.up;
            Vector3 pushForce = GetCurrentPushForce(fallDirection);
            float forceFactor = _initialForce + (_additionalForce * i);

            _blockStack.Blocks[0].BlockPhysics.FallOff(pushForce, forceFactor);
            _blockStack.Destroy(_blockStack.Blocks[0], _blockDestroyDelay);
        }
    }

    public override void Cancel()
    {
    }

    private Vector3 GetCurrentPushForce(Vector3 fallDirection)
    {
        float forceMultiplier = 1f / _blockStack.Blocks.Count;

        return _maxPushForce * forceMultiplier * fallDirection;
    }
}