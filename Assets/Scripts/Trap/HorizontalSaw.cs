using UnityEngine;

public class HorizontalSaw : Saw
{
    [SerializeField] private float _defaultSpeed = 1.4f;
    [SerializeField] private bool _canRandomizeSpeed;
    [SerializeField] private SawSide _sawSide;
    [SerializeField] private RotationDirection _rotationDirection;

    public SawSide Side => _sawSide;

    protected override void SetSpeed()
    {
        if (_canRandomizeSpeed)
        {
            float speed = Random.Range(MinSpeed, MaxSpeed) * GetRotationDirection();

            if (_rotationDirection == RotationDirection.Forward)
            {
                Animator.SetFloat(SpeedHash, speed);
                return;
            }

            Animator.SetFloat(SpeedHash, -speed);
        }
        else
        {
            if (_rotationDirection == RotationDirection.Forward)
            {
                Animator.SetFloat(SpeedHash, _defaultSpeed * GetRotationDirection());
                return;
            }

            Animator.SetFloat(SpeedHash, -_defaultSpeed * GetRotationDirection());
        }
    }

    protected override void CompleteCollision(Vector3 contactPoint, Cubic cubic)
    {
        cubic.SoundSystem.Play(SoundEvent.Sawing);
        StartCoroutine(CheckIntersectionWithCubic(cubic));
        CameOut += OnSawCameOutCubic;
        Collider.isTrigger = false;
    }

    private int GetRotationDirection()
    {
        const float CenterPositionZ = 0f;
        const int ForwardRotationValue = 1;
        const int ReverseRotationValue = -1;

        if (_sawSide == SawSide.Right)
        {
            return transform.position.z < CenterPositionZ ? ForwardRotationValue : ReverseRotationValue;
        }

        return transform.position.z > CenterPositionZ ? ForwardRotationValue : ReverseRotationValue;
    }

    private void OnSawCameOutCubic(Cubic cubic)
    {
        CameOut -= OnSawCameOutCubic;
        cubic.SplitIntoPieces();
    }
}

public enum SawSide
{
    Left,
    Right
}

public enum RotationDirection
{
    Forward,
    Reverse
}