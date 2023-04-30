using UnityEngine;

public class HorizontalSaw : Saw
{
    [SerializeField] private float _defaultSpeed = 1.4f;
    [SerializeField] private bool _canRandomizeSpeed;
    [SerializeField] private SawSide _sawSide;

    private enum SawSide { Left, Right }

    protected override void SetSpeed()
    {
        if (_canRandomizeSpeed)
        {
            float speed = Random.Range(MinSpeed, MaxSpeed) * GetRotationDirection();
            Animator.SetFloat(SpeedId, speed);
        }
        else
        {
            Animator.SetFloat(SpeedId, _defaultSpeed * GetRotationDirection());
            return;
        }
    }

    private int GetRotationDirection()
    {
        const float CenterPositionZ = 0;
        const int RightRotationValue = 1;
        const int LeftRotationValue = -1;

        if (_sawSide == SawSide.Right)
        {
            if(transform.position.z < CenterPositionZ)
            {
                return RightRotationValue;
            }

            return LeftRotationValue;
        }

        if (transform.position.z > CenterPositionZ)
        {
            return RightRotationValue;
        }

        return LeftRotationValue;
    }
}
