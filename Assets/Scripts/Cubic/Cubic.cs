using System;
using UnityEngine;
using UnityEngine.UIElements;

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

    public event Action<Vector3, float> Hit;
    public event Action<PressStand> SteppedOnStand;

    public Bounds Bounds => _meshRenderer.bounds;
    public bool CanDestroy => _canDestroy;
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

    public void HitTrap(Trap trap, Vector3 contactPoint, float trapHeight)
    {
        if (trap.TryGetComponent(out Saw saw))
        {
            IsSawing = IsStartSawing(saw, contactPoint);
        }

        Hit?.Invoke(contactPoint, trapHeight);
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

    private bool IsStartSawing(Saw saw, Vector3 contactPoint)
    {
        if (saw is VerticalSaw)
        {
            if (Mathf.Approximately(transform.position.z, contactPoint.z))
            {
                return true;
            }

            return false;
        }

        return true;
    }
}