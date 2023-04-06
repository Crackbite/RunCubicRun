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
    [SerializeField] private float _crushedSizeY = .1f;

    private Collider _collider;
    private MeshRenderer _meshRenderer;

    private bool _steppedOnStand;

    public event Action Hit;
    public event Action<PressStand> SteppedOnStand;

    public Bounds Bounds => _meshRenderer.bounds;
    public bool CanDestroy => _canDestroy;
    public Trap CollisionTrap { get; private set; }
    public bool IsSawing { get; private set; }
    public float JumpAcceleration => _jumpAcceleration;
    public float JumpForce => _jumpForce;
    public float CrushedSizeY => _crushedSizeY;

    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _collider = GetComponent<Collider>();
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

    public void HitTrap(Trap trap)
    {
        CollisionTrap = trap;

        if (trap.TryGetComponent(out Saw saw))
        {
            if (trap.IsSideCollision == false)
            {
                IsSawing = true;
            }
        }

        Hit?.Invoke();
    }

    public void SplitIntoPieces(Saw saw)
    {
        if (saw is VerticalSaw)
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

    public void FlattenOut(float standMaxY)
    {
        _collider.enabled = false;
        float positionY = standMaxY + _crushedSizeY / 2;
        transform.localScale = new Vector3(transform.localScale.x, _crushedSizeY, transform.localScale.z);
        transform.position = new Vector3(transform.position.x, positionY, transform.position.z);
    }
}