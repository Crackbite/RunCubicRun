using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(BlockStacker))]
public class BlockStackerAnimator : MonoBehaviour
{
    [SerializeField] private float _scatter = 30f;
    [SerializeField] private float _speed = .1f;
    [SerializeField] private int _loops = 4;

    private BlockStacker _blockStacker;

    private void Awake()
    {
        _blockStacker = GetComponent<BlockStacker>();
    }

    private void OnEnable()
    {
        _blockStacker.BlockAdded += OnBlockAdded;
    }

    private void OnDisable()
    {
        _blockStacker.BlockAdded -= OnBlockAdded;
    }

    private void OnBlockAdded(Transform block)
    {
        Vector3 blockScale = block.localScale;

        float scatter = 1f + (_scatter / 100f);
        float scaleX = blockScale.x * scatter;
        float scaleZ = blockScale.z * scatter;
        var newScale = new Vector3(scaleX, blockScale.y, scaleZ);

        block.DOScale(newScale, _speed).SetEase(Ease.Flash).SetLoops(_loops, LoopType.Yoyo);
    }
}