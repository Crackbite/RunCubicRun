using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Trap : MonoBehaviour
{
    [SerializeField] private GameObject _splitBody;
    [SerializeField] private GameObject _wholeBody;
    [SerializeField] protected Animator Animator;
    [SerializeField] protected float MinSpeed = .6f;
    [SerializeField] protected float MaxSpeed = 1.2f;
    [SerializeField] protected float _explosionForce = 50f;
    [SerializeField] protected float _explosionRadius = 5f;

    private Collider[] _piecesColliders;
    private Rigidbody[] _piecesRigidbody;
    private Collider _collider;
    private bool _isCubicCollided;

    protected bool IsSideCollision;
    protected readonly int SpeedId = Animator.StringToHash("Speed");

    private void Awake()
    {
        _piecesRigidbody = _splitBody.GetComponentsInChildren<Rigidbody>();
        _piecesColliders = GetComponentsInChildren<Collider>();
        _collider = GetComponent<Collider>();
        SetSpeed();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent(out Cubic cubic))
        {
            _isCubicCollided = true; ;

            if (cubic.CanDestroy)
            {
                Break();
                return;
            }
            Vector3 contactPoint = collision.ClosestPoint(transform.position);
            float trapHeight = _collider.bounds.max.y;
            IsSideCollision = Mathf.Approximately(cubic.transform.position.z, Mathf.Round(contactPoint.z)) == false;
            cubic.HitTrap(this, contactPoint, trapHeight);
            CompleteCollision();
        }
        else if (collision.TryGetComponent(out ColorBlock _) && _isCubicCollided == false)
        {
            _collider.isTrigger = false;
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

        Stop();
        _wholeBody.SetActive(false);
        _splitBody.SetActive(true);

        foreach (Collider currentCollider in _piecesColliders)
        {
            currentCollider.isTrigger = false;
        }

        foreach (Rigidbody piece in _piecesRigidbody)
        {
            piece.isKinematic = false;
            Destroy(piece.gameObject, PieceLifeTime);
        }
    }
}