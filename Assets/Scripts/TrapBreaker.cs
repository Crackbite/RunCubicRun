using UnityEngine;

[RequireComponent(typeof(Trap))]

public class TrapBreaker : MonoBehaviour
{
    [SerializeField] private Trap _trap;
    [SerializeField] private GameObject _crushedObject;
    [SerializeField] private float _breakForce;
    [SerializeField] private float _minBreakDirectionX = 1f;

    private Rigidbody[] _pieces;

    private void OnEnable()
    {
        _trap.Destroying += OnTrapDestoying;
    }

    private void OnDisable()
    {
        _trap.Destroying -= OnTrapDestoying;
    }


    private void Start()
    {
        _pieces = _crushedObject.GetComponentsInChildren<Rigidbody>();
    }

    private void OnTrapDestoying()
    {
        Crash();
    }

    private void Crash()
    {
        Vector3 force;

        gameObject.SetActive(false);
        _crushedObject.SetActive(true);

        foreach (var piece in _pieces)
        {
            piece.isKinematic = false;
            force = (piece.transform.position - _crushedObject.transform.position).normalized;
            force.y = 0;

            if (force.x < _minBreakDirectionX)
                force.x = _minBreakDirectionX;

            piece.AddForce(force * _breakForce, ForceMode.Force);
            Destroy(piece.gameObject, 10);
        }
    }
}
