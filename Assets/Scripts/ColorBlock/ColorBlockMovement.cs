using UnityEngine;

[RequireComponent(typeof(ColorBlock))]
public class ColorBlockMovement : MonoBehaviour
{
    private ColorBlock _colorBlock;
    private BlockMovementCoordinator _coordinator;
    private float _followedDistanceY;
    private float _lastPlacePositionY;
    private float _runningTime;
    private bool _isPlaced = true;

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
            if (_coordinator.IsAnchorGrounded() == false)
            {
                if (_coordinator.IsFallDawn)
                {
                    currentPosition.y = _coordinator.GetYPositionInFall(_followedDistanceY);
                }
                else
                {
                    _isPlaced = false;
                    _lastPlacePositionY = currentPosition.y;
                }
            }
            else if (currentPosition.y != _lastPlacePositionY)
            {
                _followedDistanceY = _coordinator.GetDistanceFromGround(currentPosition.y);
                _lastPlacePositionY = currentPosition.y;
                _colorBlock.SetStackPosition();
            }
        }
        else
        {
            currentPosition.y = _coordinator.GetYPositionInJump(ref _runningTime, _lastPlacePositionY, _colorBlock.StackPosition);

            if (currentPosition.y < _lastPlacePositionY)
            {
                currentPosition.y = _lastPlacePositionY;
                _runningTime = 0;
                _isPlaced = true;
            }
        }

        transform.position = _coordinator.Coordinate(currentPosition, _colorBlock.StackPosition);
    }

    public void StartFollowing(BlockMovementCoordinator coordinator)
    {
        _coordinator = coordinator;
        _lastPlacePositionY = transform.position.y;
        _followedDistanceY = _coordinator.GetDistanceFromGround(transform.position.y);
    }
}