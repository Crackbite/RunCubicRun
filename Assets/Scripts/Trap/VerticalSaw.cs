using UnityEngine;

public class VerticalSaw : Saw
{
    private const int MinPatrolSpeed = 0;

    protected override void CompleteCollision()
    {
        if (IsSideCollision)
        {
            Stop();
        }
        else if (Type == TrapType.PatrolSaw)
        {
            Animator.SetFloat(SpeedId, MinPatrolSpeed);
        }

        Collider.isTrigger = false;
    }

    protected override void SetSpeed()
    {
        if (Type == TrapType.PatrolSaw)
        {
            base.SetSpeed();
        }
        else
        {
            Animator.SetFloat(SpeedId, MinPatrolSpeed);
        }
    }
}
