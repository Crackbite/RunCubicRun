using UnityEngine;

public class HorizontalSaw : Saw
{
    [SerializeField] private float _defaultSpeed = 1.4f;
    [SerializeField] private bool _canRandomizeSpeed;
    [SerializeField] private SawSide _sawSide;

    public SawSide Side => _sawSide;

    protected override void SetSpeed()
    {
        if (_canRandomizeSpeed)
        {
            float speed = Random.Range(MinSpeed, MaxSpeed) * GetRotationDirection();
            Animator.SetFloat(SpeedHash, speed);
        }
        else
        {
            Animator.SetFloat(SpeedHash, _defaultSpeed * GetRotationDirection());
        }
    }

    private int GetRotationDirection()
    {
        const float CenterPositionZ = 0f;
        const int RightRotationValue = 1;
        const int LeftRotationValue = -1;

        if (_sawSide == SawSide.Right)
        {
            return transform.position.z < CenterPositionZ ? RightRotationValue : LeftRotationValue;
        }

        return transform.position.z > CenterPositionZ ? RightRotationValue : LeftRotationValue;
    }
}

public enum SawSide
{
    Left,
    Right
}