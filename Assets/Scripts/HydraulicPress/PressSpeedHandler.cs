using UnityEngine;

public class PressSpeedHandler : MonoBehaviour
{
    [SerializeField] private BlocksContainer _blocksContainer;
    [SerializeField] private float _maxSpeed = 5f;
    [SerializeField] private float _minSpeed = 1f;
    [SerializeField] private int _minBlocksForMinSpeed = 10;
    [SerializeField] private float _cubicPressSpeed = 5f;
    [SerializeField] private bool _debugLog;

    private int _initBlockCount = -1;

    public float CubicPressSpeed => _cubicPressSpeed;

    public float GetCurrentSpeed()
    {
        if (_initBlockCount < 0)
        {
            _initBlockCount = _blocksContainer.BlocksCount;
        }

        float currentSpeed;

        if (_initBlockCount <= _minBlocksForMinSpeed)
        {
            currentSpeed = _minSpeed;
        }
        else
        {
            float lerpFactor = Mathf.InverseLerp(_initBlockCount, _minBlocksForMinSpeed, _blocksContainer.BlocksCount);
            currentSpeed = Mathf.Lerp(_maxSpeed, _minSpeed, lerpFactor);
        }

        if (_debugLog)
        {
            Debug.Log($"{_blocksContainer.BlocksCount} = {currentSpeed}");
        }

        return currentSpeed;
    }
}