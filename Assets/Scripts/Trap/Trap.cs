using UnityEngine;

[RequireComponent(typeof(Collider))]

public class Trap : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _splitBody;

    private Rigidbody[] _pieces;
    private Collider[] _colliders;

    protected bool IsSideCollision;

    private void Start()
    {
        _pieces = _splitBody.GetComponentsInChildren<Rigidbody>();
        _colliders = GetComponentsInChildren<Collider>();
    }

    public void Break()
    {
        float pieceLifeTime = 10;

        Stop();
        gameObject.SetActive(false);
        _splitBody.SetActive(true);

        foreach (var piece in _pieces)
        {
            piece.isKinematic = false;
            Destroy(piece.gameObject, pieceLifeTime);
        }
    }

    public void Stop()
    {
        _animator.enabled = false;

        foreach (var collider in _colliders)
            collider.isTrigger = false;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Cubic>(out Cubic cubic))
        {
            if (cubic.CanDestroy)
            {
                Break();
            }
            else
            {
                IsSideCollision = Mathf.Abs(transform.position.z - cubic.transform.position.z) >= transform.localScale.z / 2;
                Stop();
                cubic.HitTrap(this, IsSideCollision);
            }
        }
    }
}