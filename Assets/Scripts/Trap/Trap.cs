using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Trap : MonoBehaviour
{
    [SerializeField] private GameObject _splitBody;
    [SerializeField] private GameObject _wholeBody;
    [SerializeField] protected Animator Animator;
    [SerializeField] protected float MinSpeed = .6f;
    [SerializeField] protected float MaxSpeed = 1.2f;

    private Rigidbody[] _piecesRigidbody;
    private Collider _collider;
    private bool _isCubicCollided;
    private const float Threshold = 0.001f;

    protected readonly int SpeedId = Animator.StringToHash("Speed");

    public bool IsSideCollision { get; private set; }

    private void Awake()
    {
        _piecesRigidbody = _splitBody.GetComponentsInChildren<Rigidbody>();
        _collider = GetComponent<Collider>();
        SetSpeed();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent(out Cubic cubic))
        {
            _isCubicCollided = true;

            if (cubic.CanDestroy)
            {
                Break();
                return;
            }

            Vector3 contactPoint = _collider.ClosestPoint(cubic.transform.position);
            float trapHeight = _collider.bounds.max.y;
            IsSideCollision = Mathf.Abs(cubic.transform.position.z - contactPoint.z) > Threshold;
            cubic.HitTrap(this, contactPoint, trapHeight);
            CompleteCollision();
        }
        else if (collision.TryGetComponent(out ColorBlock block) && block.CanFollow == false)
        {
            if (_isCubicCollided == false)
            {
                _collider.isTrigger = false;
            }
        }
    }

    protected void Stop()
    {
        if (Animator != null)
        {
            Animator.enabled = false;
        }

        _collider.isTrigger = false;
    }

    protected virtual void CompleteCollision()
    {
        if (this is not Saw)
        {
            Stop();
        }
    }

    protected virtual void SetSpeed()
    {
        float speed = Random.Range(MinSpeed, MaxSpeed);
        Animator.SetFloat(SpeedId, speed);
    }

    private void Break()
    {
        const float PieceLifeTime = 10f;
        const float DragDistance = 2f;
        const float DragDuration = 1f;

        Stop();
        _wholeBody.SetActive(false);
        _splitBody.SetActive(true);

        foreach (Rigidbody piece in _piecesRigidbody)
        {
            piece.isKinematic = false;
            Vector3 randomDirection = Random.onUnitSphere;

            if (randomDirection.z > 0)
            {
                piece.transform.DOMoveZ((piece.transform.position.z + DragDistance), DragDuration).SetEase(Ease.OutQuad);
            }
            else
            {
                piece.transform.DOMoveZ((piece.transform.position.z - DragDistance), DragDuration).SetEase(Ease.OutQuad);
            }

            Destroy(piece.gameObject, PieceLifeTime);
        }
    }
}