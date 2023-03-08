using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Trap))]

public class TrapBreaker : MonoBehaviour
{
    [SerializeField] private Trap _trap;
    [SerializeField] private GameObject _wholeObject;
    [SerializeField] private GameObject _crushedObject;
    [SerializeField] private float _breakForce;
    [SerializeField] private float _minRotation = -360f;
    [SerializeField] private float _maxRotation = 360f;
    [SerializeField] private float _minRotationDuration = 1f;
    [SerializeField] private float _maxRotationDuration = 3f;
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
        //_crushedObject.SetActive(false);
    }

    private void OnTrapDestoying()
    {
        Crash();
    }

    private void Crash()
    {
        Vector3 force;

        _wholeObject.SetActive(false);
        _crushedObject.SetActive(true);

        foreach (var piece in _pieces)
        {           
            piece.isKinematic = false;
            force = (piece.transform.position - _crushedObject.transform.position).normalized;
            force.y = Mathf.Abs(force.y);

            if (force.x < _minBreakDirectionX)
                force.x = _minBreakDirectionX;

            piece.AddForce(force * _breakForce, ForceMode.Force);
            piece.transform.DORotate(new Vector3(GetAxisRotation(), GetAxisRotation(), GetAxisRotation()), Random.Range(_minRotationDuration,_maxRotationDuration));
            Destroy(piece, 3);
        }
    }

    private float GetAxisRotation()
    {
        return Random.Range(_minRotation, _maxRotation);
    }
}
