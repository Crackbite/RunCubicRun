using UnityEngine;

[RequireComponent(typeof(Collider))]

public class Trap : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _splitBody;

    private Rigidbody[] _pieces;
    private Collider[] _colliders;

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
}
