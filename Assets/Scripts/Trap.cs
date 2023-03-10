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
    }
}
