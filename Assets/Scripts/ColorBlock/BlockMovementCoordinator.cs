using UnityEngine;

public class BlockMovementCoordinator : MonoBehaviour
{
    [SerializeField] private float _followSpeed = 350f;
    [SerializeField] private float _gapSizeFactor = 0.001f;
    [SerializeField] private Cubic _cubic;

    private Transform _anchor;
    private float _anchorGroundPositionY;
    private float _deltaY;

    public bool IsFallDawn { get; private set; }

    private void Start()
    {
        _anchor = _cubic.transform;
        _anchorGroundPositionY = _anchor.position.y;
    }

    private void Update()
    {
        IsFallDawn = _anchor.position.y < _anchorGroundPositionY;
    }

    public bool IsAnchorGrounded()
    {
        _deltaY = _anchor.position.y - _anchorGroundPositionY;

        if (_deltaY < 0)
        {
            IsFallDawn = true;
        }

        return _deltaY == 0;
    }

    public float GetDistanceFromGround(float positionY)
    {
        return positionY - _anchorGroundPositionY;
    }

    public float GetYPositionInFall(float followedDistance)
    {
        return _anchor.position.y + followedDistance;
    }

    public float GetYPositionInJump(ref float time, float lastPlacePositionY, int stackPosition)
    {
        time += Time.deltaTime;
        float jumpForce = _cubic.JumpForce + (_gapSizeFactor * Mathf.Pow(stackPosition, 2f));
        float nextPositionY = (lastPlacePositionY + (jumpForce * time))
                            - (_cubic.JumpAcceleration * Mathf.Pow(time, 2f) / 2f);

        return nextPositionY;
    }

    public Vector3 Coordinate(Vector3 currentPosition, int stackPosition)
    {
        float interpolationZ = _followSpeed / stackPosition * Time.deltaTime;

        return new Vector3(
            currentPosition.x,
            currentPosition.y,
            Mathf.Lerp(currentPosition.z, _anchor.position.z, interpolationZ));
    }
}
