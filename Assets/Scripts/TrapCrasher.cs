using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Trap))]

public class TrapCrasher : MonoBehaviour
{
    [SerializeField] private Trap _trap;
    [SerializeField] private GameObject _wholeObject;
    [SerializeField] private GameObject _fragmentedObject;
    [SerializeField] private float _crashForce;

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
       // Time.timeScale = 0.1f;
        _pieces = _fragmentedObject.GetComponentsInChildren<Rigidbody>();
        _fragmentedObject.SetActive(false);
    }

    private void OnTrapDestoying()
    {
        Crash();
    }

    private void Crash()
    {
        Vector3 force;

        _wholeObject.SetActive(false);
        _fragmentedObject.SetActive(true);

        foreach (var piece in _pieces)
        {           
            piece.isKinematic = false;
            force = (piece.transform.position - _fragmentedObject.transform.position).normalized;
            force = new Vector3(Mathf.Abs(force.x), force.y, force.z);
            piece.AddForce(force * _crashForce, ForceMode.Force);
            piece.transform.DORotate(new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)), Random.Range(1,3));
            Destroy(piece, 3);
        }
    }
}
