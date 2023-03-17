using DG.Tweening;
using UnityEngine;

public class BarometerArrow : MonoBehaviour
{
    [SerializeField] private BlocksContainer _blocksContainer;
    [SerializeField] private float _minRotationY = 34f;
    [SerializeField] private float _maxRotationY = -236f;
    [SerializeField] private float _rotateDuration = .5f;

    private int _initBlockCount;

    private void OnEnable()
    {
        _blocksContainer.BlockRemoved += BlocksContainerOnBlockRemoved;
    }

    private void OnDisable()
    {
        _blocksContainer.BlockRemoved -= BlocksContainerOnBlockRemoved;
    }

    public void Init()
    {
        _initBlockCount = _blocksContainer.BlocksCount;

        Vector3 eulerAngles = transform.localEulerAngles;
        eulerAngles.y = _minRotationY;
        transform.localEulerAngles = eulerAngles;
    }

    private void BlocksContainerOnBlockRemoved()
    {
        float lerpFactor = Mathf.InverseLerp(_initBlockCount, 0f, _blocksContainer.BlocksCount);
        float rotationY = Mathf.Lerp(_minRotationY, _maxRotationY, lerpFactor);

        Vector3 newRotation = transform.localEulerAngles;
        newRotation.y = rotationY;

        transform.DOLocalRotate(newRotation, _rotateDuration);
    }
}