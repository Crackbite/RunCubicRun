using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Trap : MonoBehaviour
{
    [SerializeField] private TrapType _type;
    [SerializeField] private GameObject _splitBody;
    [SerializeField] private GameObject _wholeBody;
    [SerializeField] protected Animator Animator;
    [SerializeField] protected float MinSpeed = .6f;
    [SerializeField] protected float MaxSpeed = 1.2f;

    protected readonly int SpeedHash = Animator.StringToHash("Speed");

    protected Collider Collider;

    private bool _isCubicCollided;

    private Rigidbody[] _piecesRigidbody;

    public bool IsSideCollision { get; private set; }
    public TrapType Type => _type;

    private void Awake()
    {
        _piecesRigidbody = _splitBody.GetComponentsInChildren<Rigidbody>();
        Collider = GetComponent<Collider>();

        SetSpeed();
    }

    private void OnTriggerEnter(Collider collision)
    {
        const float Threshold = 0.001f;

        if (collision.TryGetComponent(out Cubic cubic))
        {
            _isCubicCollided = true;

            if (cubic.CanDestroy)
            {
                Break();
                return;
            }

            Vector3 cubicPosition = cubic.transform.position;
            Vector3 contactPoint = Collider.ClosestPoint(cubicPosition);
            IsSideCollision = Mathf.Abs(cubicPosition.z - contactPoint.z) > Threshold;

            float trapHeight = Collider.bounds.max.y;
            cubic.HitTrap(this, contactPoint, trapHeight);

            CompleteCollision();
        }
        else if (collision.TryGetComponent(out ColorBlock block) && block.CanFollow == false)
        {
            if (_isCubicCollided == false)
            {
                Collider.isTrigger = false;
            }
        }
    }

    protected virtual void CompleteCollision()
    {
        if (this is Saw)
        {
            return;
        }

        Stop();
        Collider.isTrigger = false;
    }

    protected virtual void SetSpeed()
    {
        float speed = Random.Range(MinSpeed, MaxSpeed);
        Animator.SetFloat(SpeedHash, speed);
    }

    protected void Stop()
    {
        if (Animator != null)
        {
            Animator.enabled = false;
        }
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

            if (randomDirection.z > 0f)
            {
                piece.transform.DOMoveZ(piece.transform.position.z + DragDistance, DragDuration).SetEase(Ease.OutQuad);
            }
            else
            {
                piece.transform.DOMoveZ(piece.transform.position.z - DragDistance, DragDuration).SetEase(Ease.OutQuad);
            }

            Destroy(piece.gameObject, PieceLifeTime);
        }
    }
}