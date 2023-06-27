using UnityEngine;

[RequireComponent(typeof(ColorBlock))]
public class ColorBlockMovement : MonoBehaviour
{
    private ColorBlock _colorBlock;
    private float _followedDistanceY;
    private bool _isPlaced = true;
    private float _lastPlacePositionY;
    private float _runningTime;
    private BlockStackCoordinator _stackCoordinator;

    private void Awake()
    {
        _colorBlock = GetComponent<ColorBlock>();
    }

    private void Update()
    {
        if (_colorBlock.CanFollow == false)
        {
            return;
        }

        Vector3 currentPosition = transform.position;

        if (_isPlaced)
        {
            UpdateBlockPositionOnGround(ref currentPosition);
        }

        transform.position = _stackCoordinator.Coordinate(currentPosition, _colorBlock.StackPosition);
    }

    private void FixedUpdate()
    {
        if (_isPlaced)
        {
            return;
        }

        Vector3 currentPosition = transform.position;
        UpdateBlockPositionInAir(ref currentPosition);
        transform.position = _stackCoordinator.Coordinate(currentPosition, _colorBlock.StackPosition);
    }

    public void StartFollowing(BlockStackCoordinator stackCoordinator)
    {
        Vector3 position = transform.position;

        _stackCoordinator = stackCoordinator;
        _lastPlacePositionY = position.y;
        _followedDistanceY = _stackCoordinator.GetDistanceFromGround(position.y);
    }

    private void UpdateBlockPositionInAir(ref Vector3 currentPosition)
    {
        if (_stackCoordinator.IsFallDawn)
        {
            currentPosition.y = _stackCoordinator.GetYPositionInFall(_followedDistanceY);
        }
        else
        {
            currentPosition.y = _stackCoordinator.GetYPositionInJump(
                ref _runningTime,
                _lastPlacePositionY,
                _colorBlock.StackPosition);

            if (currentPosition.y < _lastPlacePositionY)
            {
                currentPosition.y = _lastPlacePositionY;
                _runningTime = 0f;
                _isPlaced = true;
            }
        }
    }

    private void UpdateBlockPositionOnGround(ref Vector3 currentPosition)
    {
        const float Tolerance = 0.05f;

        if (_stackCoordinator.IsAnchorGrounded() == false)
        {
            _lastPlacePositionY = currentPosition.y;
            _isPlaced = false;
            UpdateBlockPositionInAir(ref currentPosition);
        }
        else if (Mathf.Abs(currentPosition.y - _lastPlacePositionY) > Tolerance)
        {
            _followedDistanceY = _stackCoordinator.GetDistanceFromGround(currentPosition.y);
            _lastPlacePositionY = currentPosition.y;
            _colorBlock.SetStackPosition();
        }
    }
}