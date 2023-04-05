using UnityEngine;

[RequireComponent(typeof(ColorBlock))]
public class ColorBlockMovement : MonoBehaviour
{
    private ColorBlock _colorBlock;
    private BlockStackCoordinator _stackCoordinator;
    private float _followedDistanceY;
    private float _lastPlacePositionY;
    private float _runningTime;
    private bool _isPlaced = true;

    private void Awake()
    {
        _colorBlock = GetComponent<ColorBlock>();
    }

    private void LateUpdate()
    {
        if (_colorBlock.CanFollow == false)
        {
            return;
        }

        Vector3 currentPosition = transform.position;

        if (_isPlaced)
        {
            if (_stackCoordinator.IsAnchorGrounded() == false)
            {
                if (_stackCoordinator.IsFallDawn)
                {
                    currentPosition.y = _stackCoordinator.GetYPositionInFall(_followedDistanceY);
                }
                else
                {
                    _isPlaced = false;
                    _lastPlacePositionY = currentPosition.y;
                }
            }
            else if (currentPosition.y != _lastPlacePositionY)
            {
                _followedDistanceY = _stackCoordinator.GetDistanceFromGround(currentPosition.y);
                _lastPlacePositionY = currentPosition.y;
                _colorBlock.SetStackPosition();
            }
        }
        else
        {
            currentPosition.y = _stackCoordinator.GetYPositionInJump(ref _runningTime, _lastPlacePositionY, _colorBlock.StackPosition);

            if (currentPosition.y < _lastPlacePositionY)
            {
                currentPosition.y = _lastPlacePositionY;
                _runningTime = 0;
                _isPlaced = true;
            }
        }

        transform.position = _stackCoordinator.Coordinate(currentPosition, _colorBlock.StackPosition);
    }

    public void StartFollowing(BlockStackCoordinator stackCoordinator)
    {
        _stackCoordinator = stackCoordinator;
        _lastPlacePositionY = transform.position.y;
        _followedDistanceY = _stackCoordinator.GetDistanceFromGround(transform.position.y);
    }
}