using UnityEngine;

public class VerticalSaw : Saw
{
    [SerializeField] private SawType _sawType;

    public enum SawType { Patrol, Static }

    private readonly int _speedParameterId = Animator.StringToHash("PatrolSpeed");
    private const int MaxPatrolSpeed = 1;
    private const int MinPatrolSpeed = 0;

    private void Start()
    {
        if (_sawType == SawType.Patrol)
        {
            Animator.SetFloat(_speedParameterId, MaxPatrolSpeed);
        }
        else
        {
            Animator.SetFloat(_speedParameterId, MinPatrolSpeed);
        }
    }

    protected override void CompleteCollision()
    {
        if (Mathf.Abs(transform.position.z - CubicPositionZ) > Threshold)
        {
            IsSideCollision = true;
            Stop();
        }
        else if (_sawType == SawType.Patrol)
        {
            Animator.SetFloat(_speedParameterId, MinPatrolSpeed);
        }
    }
}
