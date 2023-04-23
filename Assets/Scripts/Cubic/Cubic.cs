using System;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer), typeof(Collider))]
public class Cubic : MonoBehaviour
{
    [SerializeField] private VerticalSplitter _verticalSplitter;
    [SerializeField] private HorizontalSplitter _horizontalSplitter;
    [SerializeField] private float _jumpForce = 6.5f;
    [SerializeField] private float _jumpAcceleration = 7.7f;
    [SerializeField] private float _crushedSizeY = .1f;

    private Collider _collider;
    private MeshRenderer _meshRenderer;

    private bool _steppedOnStand;

    public event Action<Bonus> BonusReceived;
    public event Action<Vector3, float> Hit;
    public event Action<PressStand> SteppedOnStand;

    public Bounds Bounds => _meshRenderer.bounds;
    public Saw CollisionSaw { get; private set; }
    public bool CanDestroy { get; set; }
    public float CrushedSizeY => _crushedSizeY;
    public bool IsSawing { get; private set; }
    public float JumpAcceleration => _jumpAcceleration;
    public float JumpForce => _jumpForce;

    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _collider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent(out Bonus bonus))
        {
            BonusReceived?.Invoke(bonus);
            return;
        }

        if (collision.TryGetComponent(out Wall _))
        {
            Vector3 contactPoint = collision.ClosestPoint(transform.position);
            float wallHeight = collision.bounds.max.y;
            Hit?.Invoke(contactPoint, wallHeight);
            return;
        }

        if (_steppedOnStand || collision.TryGetComponent(out PressStand pressStand) == false)
        {
            return;
        }

        _steppedOnStand = true;
        SteppedOnStand?.Invoke(pressStand);
    }

    public void FlattenOut(float standMaxY)
    {
        _collider.enabled = false;
        float positionY = standMaxY + _crushedSizeY / 2;
        transform.localScale = new Vector3(transform.localScale.x, _crushedSizeY, transform.localScale.z);
        transform.position = new Vector3(transform.position.x, positionY, transform.position.z);
    }

    public void HitTrap(Trap trap, Vector3 contactPoint, float trapHeight)
    {
        if (trap.TryGetComponent(out Saw saw) && IsSawing == false)
        {
            CollisionSaw = saw;
            IsSawing = IsStartSawing();
            Hit?.Invoke(contactPoint, trapHeight);
        }
        else
        {
            Hit?.Invoke(contactPoint, trapHeight);
        }
    }

    public void SplitIntoPieces()
    {
        if (CollisionSaw is VerticalSaw)
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

    private bool IsStartSawing()
    {
        if (CollisionSaw is VerticalSaw)
        {
            if (CollisionSaw.IsSideCollision == false)
            {
                return true;
            }

            return false;
        }

        return true;
    }
}