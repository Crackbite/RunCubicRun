using DG.Tweening;
using UnityEngine;

public class BlockStackerAnimator : MonoBehaviour
{
    [SerializeField] private float _scatter = 30f;
    [SerializeField] private float _speed = .1f;
    [SerializeField] private Ease _ease = Ease.Flash;
    [SerializeField] private int _loops = 4;
    [SerializeField] private LoopType _loopType = LoopType.Yoyo;
    [SerializeField] private ColorBlockCollection _blockCollection;

    private void OnEnable()
    {
        _blockCollection.BlockAdded += OnBlockAdded;
    }

    private void OnDisable()
    {
        _blockCollection.BlockAdded -= OnBlockAdded;
    }

    private void OnBlockAdded(ColorBlock colorBlock)
    {
        Vector3 blockScale = colorBlock.transform.localScale;

        float scatter = 1f + (_scatter / 100f);
        float scaleX = blockScale.x * scatter;
        float scaleZ = blockScale.z * scatter;
        var newScale = new Vector3(scaleX, blockScale.y, scaleZ);

        colorBlock.transform.DOScale(newScale, _speed).SetEase(_ease).SetLoops(_loops, _loopType);
    }
}