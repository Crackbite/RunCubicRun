using UnityEngine;

public class FollowController : MonoBehaviour
{
    [SerializeField] private float _followSpeed = 350f;
    [SerializeField] private float _gapSizeFactor = 0.001f;
    [SerializeField] private Cubic _cubic;

    private Transform _followed;
    private float _followedGroundPositionY;
    private float _deltaY;

    public bool IsFallDawn { get; private set; }

    private void Start()
    {
        _followed = _cubic.transform;
        _followedGroundPositionY = _followed.position.y;
    }

    private void Update()
    {
        IsFallDawn = _followed.position.y < _followedGroundPositionY;
    }

    public bool CheckGround()
    {
        _deltaY = _followed.position.y - _followedGroundPositionY;

        if(_deltaY < 0 )
        {
            IsFallDawn = true;
        }

        return _deltaY == 0;
    }

    public float GetFollowedDistance(float positionY)
    {
        return positionY - _followedGroundPositionY;
    }

    public float GetBlockPositionY(float followedDistance)
    {
        return _followed.position.y + followedDistance;
    }

    public bool CheckIsPlaced(ref float currentPositionY, ref float time, float lastGroundPositionY, int stackPosition)
    {
        time += Time.deltaTime;
        float jumpForce = _cubic.JumpForce + (_gapSizeFactor * Mathf.Pow(stackPosition, 2f));
        currentPositionY = (lastGroundPositionY + jumpForce * time)
                            - (_cubic.JumpAcceleration * Mathf.Pow(time, 2f) / 2f);

        if (currentPositionY < lastGroundPositionY)
        {
            currentPositionY = lastGroundPositionY;
            time = 0;
            return true;
        }

        return false;
    }

    public Vector3 Follow(Vector3 currentPosition, int stackPosition)
    {
        float interpolationZ = _followSpeed / stackPosition * Time.deltaTime;

        return new Vector3(
            currentPosition.x,
            currentPosition.y,
            Mathf.Lerp(currentPosition.z, _followed.position.z, interpolationZ));
    }
}
