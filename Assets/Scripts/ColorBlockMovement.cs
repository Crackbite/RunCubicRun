using UnityEngine;

[RequireComponent(typeof(ColorBlock))]
public class ColorBlockMovement : MonoBehaviour
{
    private ColorBlock _colorBlock;
    private FollowController _followController;
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
            if (_followController.CheckGround() == false)
            {
                if (_followController.IsFallDawn)
                {
                    currentPosition.y = _followController.GetBlockPositionY(_followedDistanceY);
                }
                else
                {
                    _isPlaced = false;
                    _lastPlacePositionY = currentPosition.y;
                }
            }
            else if (currentPosition.y != _lastPlacePositionY)
            {
                _followedDistanceY = _followController.GetFollowedDistance(currentPosition.y);
                _lastPlacePositionY = currentPosition.y;
                _colorBlock.SetStackPosition();
            }
        }
        else
        {
            _isPlaced = _followController.CheckIsPlaced(ref currentPosition.y, ref _runningTime, _lastPlacePositionY, _colorBlock.StackPosition);
        }

        transform.position = _followController.Follow(currentPosition, _colorBlock.StackPosition);
    }

    public void StartFollowing(FollowController followController)
    {
        _followController = followController;
        _lastPlacePositionY = transform.position.y;
        _followedDistanceY = _followController.GetFollowedDistance(transform.position.y);
    }
}
