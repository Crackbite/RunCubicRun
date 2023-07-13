using UnityEngine;

public class VerticalSaw : Saw
{
    private const int MinPatrolSpeed = 0;

    private void OnEnable()
    {
        SetSpeed();
    }

    protected override void CompleteCollision(Vector3 contactPoint)
    {
        if (IsSideCollision)
        {
            HitEffect.transform.position = contactPoint;
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