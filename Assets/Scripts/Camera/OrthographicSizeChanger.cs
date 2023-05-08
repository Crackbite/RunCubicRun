using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class OrthographicSizeChanger : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private float _fixedHorizontalSize = 8f;
    [SerializeField] private float _fixedVerticalSize = 18f;

    private CinemachineVirtualCamera _virtualCamera;

    private void Awake()
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    private void Start()
    {
        float newOrthographicSize;
        float cameraAspect = _mainCamera.aspect;

        if (cameraAspect < 1f)
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