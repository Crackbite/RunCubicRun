using DG.Tweening;
using UnityEngine;

public class BarometerArrow : MonoBehaviour
{
    [SerializeField] private ColorBlockCollection _blockCollection;
    [SerializeField] private float _minRotationY = 34f;
    [SerializeField] private float _maxRotationY = -236f;
    [SerializeField] private float _rotateDuration = .5f;

    private int _initBlockCount;

    private void OnEnable()
    {
        _blockCollection.BlockRemoved += BlockCollectionOnBlockRemoved;
    }

    private void OnDisable()
    {
        _blockCollection.BlockRemoved -= BlockCollectionOnBlockRemoved;
    }

    public void Init()
    {
        _initBlockCount = _blockCollection.Blocks.Count;

        Vector3 eulerAngles = transform.localEulerAngles;
        eulerAngles.y = _minRotationY;
        transform.localEulerAngles = eulerAngles;
    }

    private void BlockCollectionOnBlockRemoved(ColorBlock colorBlock)
    {
        float lerpFactor = Mathf.InverseLerp(_initBlockCount, 0f, _blockCollection.Blocks.Count);
        float rotationY = Mathf.Lerp(_minRotationY, _maxRotationY, lerpFactor);

        Vector3 newRotation = transform.localEulerAngles;
        newRotation.y = rotationY;

        transform.DOLocalRotate(newRotation, _rotateDuration);
    }
}