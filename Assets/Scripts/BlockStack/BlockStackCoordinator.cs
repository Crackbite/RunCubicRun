using UnityEngine;

public class BlockStackCoordinator : MonoBehaviour
{
    [SerializeField] private Cubic _cubic;
    [SerializeField] private float _followSpeed = 350f;
    [SerializeField] private float _gapSizeFactor = 0.001f;

    private Transform _anchor;

    private float _anchorGroundPositionY;
    private float _deltaY;

    public bool IsFallDawn { get; private set; }

    private void Start()
    {
        _anchor = _cubic.transform;
        _anchorGroundPositionY = _anchor.position.y;
    }

    public Vector3 Coordinate(Vector3 currentPosition, int stackPosition)
    {
        float interpolationZ = _followSpeed / stackPosition * Time.deltaTime;

        return new Vector3(
            currentPosition.x,
            currentPosition.y,
            Mathf.Lerp(currentPosition.z, _anchor.position.z, interpolationZ));
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
        const float InitialTime = 0;

        if (time == InitialTime)
        {
            time += Time.fixedDeltaTime;
            GetYPositionInJump(ref time, lastPlacePositionY, stackPosition);
        }
        else
        {
            time += Time.fixedDeltaTime;
        }

        float jumpForce = _cubic.JumpForce + (_gapSizeFactor * Mathf.Pow(stackPosition, 2f));
        float nextPositionY = (lastPlacePositionY + (jumpForce * time))
                              - (_cubic.JumpAcceleration * Mathf.Pow(time, 2f) / 2f);

        return nextPositionY;
    }

    public bool IsAnchorGrounded()
    {
        const float Threshold = 0.001f;

        _deltaY = _anchor.position.y - _anchorGroundPositionY;

        if (_deltaY < -Threshold && IsFallDawn == false)
        {
            IsFallDawn = true;
        }

        return Mathf.Abs(_deltaY) < Threshold;
    }
}