using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class OrthographicSizeChanger : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private float _fixedHorizontalSize = 8f;
    [SerializeField] private float _fixedVerticalSize = 18f;

    private float _lastCameraAspect;
    private CinemachineVirtualCamera _virtualCamera;

    private void Awake()
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    private void LateUpdate()
    {
        float cameraAspect = _mainCamera.aspect;
        
        if (Mathf.Approximately(cameraAspect, _lastCameraAspect))
        {
            return;
        }

        ChangeOrthographicSize(cameraAspect);
        _lastCameraAspect = cameraAspect;
    }

    private void ChangeOrthographicSize(float cameraAspect)
    {
        const float UnityScale = 1f;

        float newOrthographicSize;

        if (cameraAspect < UnityScale)
        {
            newOrthographicSize = _fixedHorizontalSize / cameraAspect;
        }
        else
        {
            newOrthographicSize = _fixedVerticalSize / cameraAspect;
        }

        _virtualCamera.m_Lens.OrthographicSize = newOrthographicSize;
    }
}