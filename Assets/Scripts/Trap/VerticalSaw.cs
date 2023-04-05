using UnityEngine;

public class VerticalSaw : Saw
{
    [SerializeField] private SawType _sawType;

    public enum SawType { Patrol, Static }

    private readonly int Patrol = Animator.StringToHash("Base Layer.Patrol");
    private readonly int StaticSpin = Animator.StringToHash("Base Layer.StaticSpin");

    private void Start()
    {
        switch (_sawType)
        {
            case SawType.Patrol:
                Animator.Play(Patrol);
                break;
            case SawType.Static:
                Animator.Play(StaticSpin);
                break;
        }
    }

    protected override void CompleteCollision()
    {
        if (Mathf.Abs(transform.position.z - CubicPositionZ) > Threshold)
        {
            IsSideCollision = true;
            Stop();
        }
        else if(_sawType == SawType.Patrol)
        {
            Animator.Play(StaticSpin);
        }
    }
}
