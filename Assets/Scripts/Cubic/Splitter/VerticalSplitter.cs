using DG.Tweening;
using UnityEngine;

public class VerticalSplitter : Splitter
{
    [SerializeField] private float _splitDuration;
    [SerializeField] private float _splitAngle;
    [SerializeField] private Ease _ease = Ease.InExpo;

    public override void Split()
    {
        SplitOnePart(FirstPart);
        SplitOnePart(SecondPart);
    }

    public override void SplitOnePart(Transform part)
    {
        part.gameObject.SetActive(true);
        part.SetParent(null);
        part.DORotate(_splitAngle * Mathf.Sign(part.transform.localPosition.z) * Vector3.right, _splitDuration)
            .SetEase(_ease);
    }
}