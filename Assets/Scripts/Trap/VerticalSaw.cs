using UnityEngine;

public class VerticalSaw : Saw
{
    private const int MinPatrolSpeed = 0;

    protected override void CompleteCollision(Vector3 contactPoint)
    {
        if (IsSideCollision)
        {
            Instantiate(HitEffect, contactPoint, Quaternion.identity);
            HitEffect.Play();
            Stop();
        }
        else if (Type == TrapType.PatrolSaw)
        {
            Animator.SetFloat(SpeedHash, MinPatrolSpeed);
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
            Animator.SetFloat(SpeedHash, MinPatrolSpeed);
        }
    }
}