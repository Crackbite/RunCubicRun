using Cinemachine;
using UnityEngine;

public class MainCameraTarget : MonoBehaviour
{
    [SerializeField] private float _leftVerticalOffset = 5f;
    [SerializeField] private float _rightVerticalOffset = -8f;
    [SerializeField] private float _leftHorizontalOffset = 9f;
    [SerializeField] private float _rightHorizontalOffset = -1f;
    [SerializeField] private Cubic _cubic;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private MeshRenderer _starterRoad;
    [SerializeField] private MeshRenderer _finalRoad;
    [SerializeField] private LevelGenerationStarter _generatorStarter;
    [SerializeField] private GameStatusTracker _gameStatusTracker;
    [SerializeField] private LevelEntryPortal _levelEntryPortal;

    private bool _hasStarted;
    private bool _isPositionChangeStopped;
    private float _leftLimit;
    private float _previousCameraAspect;
    private float _rightLimit;


    private void OnEnable()
    {
        _levelEntryPortal.ThrowingOut += OnCubicTwrowingOut;
        _cubic.Hit += OnCubicHit;
    }

    private void LateUpdate()
    {
        if (_hasStarted == false)
        {
            return;
        }

        float cameraAspect = _mainCamera.aspect;

        if (Mathf.Approximately(cameraAspect, _previousCameraAspect) == false)
        {
            ChangeLimits(cameraAspect);
            _previousCameraAspect = cameraAspect;
        }

        UpdatePosition(cameraAspect);
    }

    private void OnDisable()
    {
        _levelEntryPortal.ThrowingOut -= OnCubicTwrowingOut;
        _cubic.Hit -= OnCubicHit;
    }

    private void ChangeLimits(float cameraAspect)
    {
        const float UnityScale = 1f;

        if (cameraAspect < UnityScale)
        {
            _leftLimit = _starterRoad.bounds.min.x - _leftVerticalOffset;
            _rightLimit = _finalRoad.bounds.max.x + _rightVerticalOffset;
        }
        else
        {
            _leftLimit = _starterRoad.bounds.min.x - _leftHorizontalOffset;
            _rightLimit = _finalRoad.bounds.max.x + _rightHorizontalOffset;
        }
    }

    private void OnCubicTwrowingOut()
    {
        _hasStarted = true;
    }

    private void OnCubicHit(Vector3 contactPoint, float obstacleHeight)
    {
        _isPositionChangeStopped = true;
    }

    private void UpdatePosition(float cameraAspect)
    {
        const float LimitOnY = 0f;
        const float LimitOnZ = 0f;

        if (_isPositionChangeStopped)
        {
            return;
        }

        float halfCamWidth = _mainCamera.orthographicSize * cameraAspect;

        float clampedX = Mathf.Clamp(
            _cubic.transform.position.x,
            _leftLimit + halfCamWidth,
            _rightLimit - halfCamWidth);

        transform.position = new Vector3(clampedX, LimitOnY, LimitOnZ);
    }
}