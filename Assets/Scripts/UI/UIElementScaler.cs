using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class UIElementScaler : MonoBehaviour
{
    [SerializeField] private float _fixedHorizontalSize = 8f;
    [SerializeField] private float _fixedVerticalSize = 18f;

    private const float FixedHorizontalAspect = 1.777778f;
    private const float Epsilon = 0.01f;

    private Camera _camera;
    private RectTransform _scaledElement;
    private float _ratioX;
    private float _ratioY;
    private float _lastCameraAspect;

    private void Awake()
    {
        _camera = Camera.main;
        _scaledElement = GetComponent<RectTransform>();
        _ratioX = _scaledElement.localScale.x * FixedHorizontalAspect;
        _ratioY = _scaledElement.localScale.y * FixedHorizontalAspect;
    }

    private void LateUpdate()
    {
        float cameraAspect = _camera.aspect;

        if (Mathf.Abs(cameraAspect - _lastCameraAspect) < Epsilon)
        {
            return;
        }

        Scale(cameraAspect);
        _lastCameraAspect = cameraAspect;
    }

    private void Scale(float cameraAspect)
    {
        const float UnityScale = 1f;
        const float MinScale = 0;
        const float MaxScale = 1;

        float scaleX;
        float scaleY;

        if (cameraAspect <= UnityScale)
        {
            scaleX = Mathf.Clamp(Mathf.Sqrt(_ratioX * cameraAspect), MinScale, MaxScale);
            scaleY = Mathf.Clamp(Mathf.Sqrt(_ratioY * cameraAspect), MinScale, MaxScale);
        }
        else
        {
            scaleX = Mathf.Clamp(Mathf.Sqrt(_ratioX / cameraAspect), MinScale, MaxScale);
            scaleY = Mathf.Clamp(Mathf.Sqrt(_ratioY / cameraAspect), MinScale, MaxScale);
        }

        Vector3 newScale = new Vector3(scaleX, scaleY, _scaledElement.localScale.z);
        _scaledElement.localScale = newScale;
    }
}
