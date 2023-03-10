using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _splitBody;

    private Rigidbody[] _pieces;

    private void Start()
    {
        _pieces = _splitBody.GetComponentsInChildren<Rigidbody>();
    }

    private void Break()
    {
        float pieceLifeTime = 10;

        gameObject.SetActive(false);
        _splitBody.SetActive(true);

        foreach (var piece in _pieces)
        {
            piece.isKinematic = false;
            Destroy(piece.gameObject, pieceLifeTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Cubic>(out Cubic cubic))
        {
            if (cubic.CanDestroy)
            {
                _animator.enabled = false;
                Break();
            }
        }
    }
}
