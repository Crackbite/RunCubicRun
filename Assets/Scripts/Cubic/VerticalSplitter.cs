using DG.Tweening;
using UnityEngine;

public class VerticalSplitter : Splitter
{
    [SerializeField] private float _splitDuration;
    [SerializeField] private float _splitAngle;
    [SerializeField] private Ease _ease = Ease.InExpo;

    public override void Split()
    {
        SplitOnePart(FirstPart, _splitAngle);
        SplitOnePart(SecondPart, -_splitAngle);
    }

    public void SplitOnePart(Transform part, float angle)
    {
        part.gameObject.SetActive(true);
        part.DORotate(Vector3.right * angle, _splitDuration).SetEase(_ease);
    }
}
