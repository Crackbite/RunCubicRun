using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class Cubic : MonoBehaviour
{
    [SerializeField] private VerticalSplitter _verticalSplitter;
    [SerializeField] private HorizontalSplitter _horizontalSplitter;
    [SerializeField] private float _jumpForce = 6.5f;
    [SerializeField] private float _jumpAcceleration = 7.7f;
    [SerializeField] private float _crushedSizeY = .1f;
    [SerializeField] private List<MeshRenderer> _meshRenderers;

    private Collider _collider;
    private Rigidbody _rigidbody;

    private bool _steppedOnStand;

    public event Action<Bonus> BonusReceived;
    public event Action<Vector3, float> Hit;
    public event Action<PressStand> SteppedOnStand;

    public Bounds Bounds => _meshRenderers[0].bounds;
    public bool CanDestroy { get; set; }
    public Saw CollisionSaw { get; private set; }
    public float CrushedSizeY => _crushedSizeY;
    public bool IsSawing { get; private set; }
    public float JumpAcceleration => _jumpAcceleration;
    public float JumpForce => _jumpForce;

    private void Start()
    {
        _collider = GetComponent<Collider>();
        _rigidbody = GetComponent<Rigidbody>();
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


        if (collision.TryGetComponent(out Spring _) && IsSawing)
        {
            const float PushForce = 3f;

            _rigidbody.isKinematic = false;
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            _rigidbody.AddForce(Vector3.up * PushForce, ForceMode.Impulse);
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

        Vector3 localScale = transform.localScale;
        transform.localScale = new Vector3(localScale.x, _crushedSizeY, localScale.z);

        Vector3 position = transform.position;
        float positionY = standMaxY + (_crushedSizeY / 2f);
        transform.position = new Vector3(position.x, positionY, position.z);
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

        if (IsSawing == false)
        {
            _rigidbody.isKinematic = false;
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
            foreach (MeshRenderer renderer in _meshRenderers)
            {
                renderer.gameObject.SetActive(false);
            }

            _horizontalSplitter.Split();
        }

        _collider.enabled = false;
    }

    private bool IsStartSawing()
    {
        if (CollisionSaw is VerticalSaw)
        {
            return CollisionSaw.IsSideCollision == false;
        }

        return true;
    }
}