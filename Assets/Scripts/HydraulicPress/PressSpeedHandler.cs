using UnityEngine;

public class PressSpeedHandler : MonoBehaviour
{
    [SerializeField] private BlockStack _blockStack;
    [SerializeField] private float _maxSpeed = 5f;
    [SerializeField] private float _minSpeed = 1f;
    [SerializeField] private int _minBlocksForMinSpeed = 10;
    [SerializeField] private bool _debugLog;

    private int _initBlockCount;

    public float PureSpeed { get; private set; }

    public float SpeedReduceRate { get; set; }

    public float GetCurrentSpeed()
    {
        float currentSpeed;
        float blocksCount = _blockStack.Blocks.Count;

        if (_initBlockCount <= _minBlocksForMinSpeed)
        {
            currentSpeed = _minSpeed;
        }
        else
        {
            float lerpFactor = Mathf.InverseLerp(_initBlockCount, _minBlocksForMinSpeed, blocksCount);
            currentSpeed = Mathf.Lerp(_maxSpeed, _minSpeed, lerpFactor);
        }

        PureSpeed = currentSpeed;

        float newSpeed = currentSpeed - SpeedReduceRate;
        currentSpeed = currentSpeed < 0f ? currentSpeed : newSpeed;

        if (_debugLog)
        {
            Debug.Log($"{blocksCount} = {currentSpeed} | {SpeedReduceRate}");
        }

        return currentSpeed;
    }

    public void Init()
    {
        _initBlockCount = _blockStack.Blocks.Count;
    }
}