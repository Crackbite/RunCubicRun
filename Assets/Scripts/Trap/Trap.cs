using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Trap : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _splitBody;

    protected bool IsSideCollision;

    private Collider[] _colliders;
    private Rigidbody[] _pieces;

    private void Start()
    {
        _pieces = _splitBody.GetComponentsInChildren<Rigidbody>();
        _colliders = GetComponentsInChildren<Collider>();
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
                              >= transform.localScale.z / 2f;
            Stop();
            cubic.HitTrap(this, IsSideCollision);
        }
    }

    public void Break()
    {
        const float PieceLifeTime = 10f;

        Stop();
        gameObject.SetActive(false);
        _splitBody.SetActive(true);

        foreach (Rigidbody piece in _pieces)
        {
            piece.isKinematic = false;
            Destroy(piece.gameObject, PieceLifeTime);
        }
    }

    public void Stop()
    {
        _animator.enabled = false;

        foreach (Collider currentCollider in _colliders)
        {
            currentCollider.isTrigger = false;
        }
    }
}