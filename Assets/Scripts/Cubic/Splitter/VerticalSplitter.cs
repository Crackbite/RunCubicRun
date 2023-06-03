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
        float localPositionZ = part.transform.localPosition.z;
        Rigidbody partRigidbody = part.GetComponent<Rigidbody>();
        part.DORotate(_splitAngle * Mathf.Sign(localPositionZ) * Vector3.right, _splitDuration)
            .SetEase(_ease)
            .OnComplete(() =>
            {
                if (partRigidbody != null)
                {
                    part.SetParent(null);
                    partRigidbody.isKinematic = false;
                }
            });
    }
}