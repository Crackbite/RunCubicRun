using System;
using DG.Tweening;
using UnityEngine;

public class BlockStackAddAnimator : MonoBehaviour
{
    [SerializeField] private float _scatter = 30f;
    [SerializeField] private float _duration = .1f;
    [SerializeField] private Ease _ease = Ease.Flash;
    [SerializeField] private int _loops = 4;
    [SerializeField] private LoopType _loopType = LoopType.Yoyo;

    public event Action<ColorBlock> AnimationCompleted;

    public void StartAddAnimation(ColorBlock colorBlock)
    {
        Vector3 blockScale = colorBlock.transform.localScale;

        float scatter = 1f + (_scatter / 100f);
        float scaleX = blockScale.x * scatter;
        float scaleZ = blockScale.z * scatter;
        var newScale = new Vector3(scaleX, blockScale.y, scaleZ);

        colorBlock.transform.DOScale(newScale, _duration).SetEase(_ease).SetLoops(_loops, _loopType)
            .OnComplete(() => AnimationCompleted?.Invoke(colorBlock));
    }
}