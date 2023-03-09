using DG.Tweening;
using UnityEngine;

public class PressTop : MonoBehaviour
{
    [SerializeField] private float _fallSpeed = .3f;
    [SerializeField] private Ease _fallEase = Ease.InOutFlash;

    public void EnableFallAnimation(float highestBlockHeight)
    {
        transform.DOMoveY(highestBlockHeight, _fallSpeed).SetEase(_fallEase);
    }
}