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
        const int AngleY = 180;

        if (part.gameObject.TryGetComponent<Collider>(out Collider partCollider))
        {
            partCollider.enabled = true;
        }

        Vector3 startEulerAngles = transform.localEulerAngles;
        Vector3 targetEulerAngles = new Vector3(_splitAngle, startEulerAngles.y, startEulerAngles.z);
        float localPositionZ = part.transform.localPosition.z;

        if(localPositionZ < 0)
        {
            targetEulerAngles.y = AngleY;
        }

        Rigidbody partRigidbody = part.gameObject.AddComponent<Rigidbody>();

        part.DOLocalRotate(targetEulerAngles, _splitDuration)
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