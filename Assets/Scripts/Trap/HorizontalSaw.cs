using UnityEngine;

public class HorizontalSaw : Saw
{
    [SerializeField] private SpinType _spinType;

    public enum SpinType { Right, Left }

    private readonly int RightSpin = Animator.StringToHash("Base Layer.RightSpin");
    private readonly int LeftSpin = Animator.StringToHash("Base Layer.LeftSpin");

    private void Start()
    {
        switch (_spinType)
        {
            case SpinType.Right:
                Animator.Play(RightSpin);
                break;
            case SpinType.Left:
                Animator.Play(LeftSpin);
                break;
        }
    }

    protected override void CompleteCollision()
    {
        IsSideCollision = false;
    }
}
