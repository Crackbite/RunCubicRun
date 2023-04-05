using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Trap : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _splitBody;
    [SerializeField] private GameObject _wholeBody;

    private Collider[] _colliders;
    private Rigidbody[] _pieces;
    private Collider _collider;

    protected bool IsSideCollision;
    protected const float Threshold = .1f;

    private void Awake()
    {
        _pieces = _splitBody.GetComponentsInChildren<Rigidbody>();
        _colliders = GetComponentsInChildren<Collider>();
        _collider = GetComponent<Collider>();
    }

    protected virtual void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent(out Cubic cubic) == false)
        {
            return;
        }

        if (cubic.CanDestroy)
        {
            Break();
        }
        else
        {
            IsSideCollision = Mathf.Abs(transform.position.z - cubic.transform.position.z)
                              >= Threshold;
            Stop();
            cubic.HitTrap(this, IsSideCollision);
        }
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
        _animator.enabled = false;
        _collider.isTrigger = false;
    }
}