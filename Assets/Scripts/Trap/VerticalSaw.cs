using UnityEngine;

public class VerticalSaw : Saw
{
    [SerializeField] private SawType _sawType;

    public enum SawType { Patrol, Static }

    private const int MinPatrolSpeed = 0;

    protected override void CompleteCollision()
    {
        if (Mathf.Abs(transform.position.z - CubicPositionZ) > Threshold)
        {
            IsSideCollision = true;
            Stop();
        }
        else if (_sawType == SawType.Patrol)
        {
            Animator.SetFloat(SpeedId, MinPatrolSpeed);
        }
    }

    protected override void SetSpeed()
    {
        if (_sawType == SawType.Patrol)
        {
            base.SetSpeed();
        }
        else
        {
            Animator.SetFloat(SpeedId, MinPatrolSpeed);
        }
    }
}
