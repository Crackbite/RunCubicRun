using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Trap : MonoBehaviour
{
    [SerializeField] private GameObject _splitBody;
    [SerializeField] private GameObject _wholeBody;
    [SerializeField] protected Animator Animator;

    private Collider[] _colliders;
    private Rigidbody[] _pieces;
    private Collider _collider;

    protected const float Threshold = .1f;
    protected float CubicPositionZ;

    public bool IsSideCollision { get; protected set; }

    private void Awake()
    {
        _pieces = _splitBody.GetComponentsInChildren<Rigidbody>();
        _colliders = GetComponentsInChildren<Collider>();
        _collider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent(out Cubic cubic) == false)
        {
            return;
        }

        if (cubic.CanDestroy)
        {
            Break();
            return;
        }

        CubicPositionZ = cubic.transform.position.z;
        CompleteCollision();
        cubic.HitTrap(this);
    }

    public void Break()
    {
        const float PieceLifeTime = 10f;

        Stop();
        _wholeBody.SetActive(false);
        _splitBody.SetActive(true);

        foreach (Collider currentCollider in _colliders)
        {
            currentCollider.isTrigger = false;
        }

        foreach (Rigidbody piece in _pieces)
        {
            piece.isKinematic = false;
            Destroy(piece.gameObject, PieceLifeTime);
        }
    }
    public void Stop()
    {
        Animator.enabled = false;
        _collider.isTrigger = false;
    }

    protected virtual void CompleteCollision()
    {
        IsSideCollision = Mathf.Abs(transform.position.z - CubicPositionZ)
                          >= Threshold;
        Stop();
    }
}