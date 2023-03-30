using UnityEngine;
using DG.Tweening;

public abstract class HyperspacePortal : MonoBehaviour
{
    [SerializeField] protected Cubic Cubic;
    [SerializeField] protected Transform Center;
    [SerializeField] protected float FlightDuration = 1.2f; 
    [SerializeField] protected float RotationAngle = 360f; 
    [SerializeField] protected float RotationSpeed = 2f;
    [SerializeField] protected Ease FlightEase = Ease.InCubic; 
    [SerializeField] protected Ease RotationEase = Ease.OutQuad;

    protected Vector3 TargetScale;
    protected Sequence FlightSequence;
    protected float TargetPositionY;
    protected Transform CubicTransform;

    private void Awake()
    {
        CubicTransform = Cubic.transform;
    }
}

