using System;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer), typeof(Collider))]

public class Cubic : MonoBehaviour
{
    [SerializeField] private bool _canDestroy;
    [SerializeField] private VerticalSplitter _verticalSplitter;
    [SerializeField] private HorizontalSplitter _horizontalSplitter;
    [SerializeField] private float _jumpForce = 6.5f;
    [SerializeField] private float _jumpAcceleration = 7.7f;

    private MeshRenderer _meshRenderer;
    private bool _steppedOnStand;
    private Collider _collider;

    public event Action<PressStand> SteppedOnStand;

    public Bounds Bounds => _meshRenderer.bounds;
    public bool CanDestroy => _canDestroy;
    public bool IsSawing { get; private set; }
    public bool IsSideCollision { get; private set; }
    public Trap CollisionTrap { get; private set; }
    public float JumpForce => _jumpForce;
    public float JumpAcceleration => _jumpAcceleration;

    public event Action Hit;

    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _collider = GetComponent<Collider>();
    }

    public void SplitIntoPieces(bool isVerticalSplit)
    {
        if (isVerticalSplit)
        {
            _verticalSplitter.Split();
        }
        else
        {
            _horizontalSplitter.Split();
        }

        _meshRenderer.enabled = false;
        _collider.enabled = false;
    }

    public void HitTrap(Trap trap, bool isSideCollision)
    {
        CollisionTrap = trap;
        IsSideCollision = isSideCollision;

        if (trap.TryGetComponent<Saw>(out Saw saw))
        {
            if (saw.IsVertical == false || (saw.IsVertical && IsSideCollision == false))
            {
                IsSawing = true;
            }
        }

        Hit?.Invoke();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent(out Wall _))
        {
            Hit?.Invoke();
            return;
        }

        if (_steppedOnStand || collision.TryGetComponent(out PressStand pressStand) == false)
        {
            return;
        }

        _steppedOnStand = true;
        SteppedOnStand?.Invoke(pressStand);
    }

}