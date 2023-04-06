using UnityEngine;

public abstract class HyperspacePortal : MonoBehaviour
{
    [SerializeField] protected Cubic Cubic;
    [SerializeField] protected Transform Center;
    [SerializeField] protected float Delay = 1f;
    [SerializeField] protected float FlightDuration = 1.2f;
    [SerializeField] protected float RotationAngle = 360f;
    [SerializeField] protected float RotationSpeed = 2f;

    protected Transform CubicTransform;
    protected Vector3 TargetScale;

    private void Awake()
    {
        CubicTransform = Cubic.transform;
    }
}