using System;
using DG.Tweening;
using UnityEngine;

public class BlockStackDestroyAnimator : MonoBehaviour
{
    [SerializeField] private float _duration = .2f;
    [SerializeField] private Ease _ease = Ease.Flash;

    public event Action<ColorBlock> AnimationCompleted;

    public void StartDestroyAnimation(ColorBlock colorBlock, float delay = 0f)
    {
        colorBlock.transform.DOScale(Vector3.zero, _duration).SetEase(_ease)
            .OnComplete(() => AnimationCompleted?.Invoke(colorBlock)).SetDelay(delay);
    }
}