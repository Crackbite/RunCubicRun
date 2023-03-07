using System.Collections;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Cubic))]
public class CubicMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _animationSpeed = .1f;
    [SerializeField] private float _shiftPerMove = 1.3f;
    
    private bool _canMove;
    private Cubic _cubic;
    private float _maxPositionZ;
    private float _minPositionZ;

    private void Start()
    {
        _cubic = GetComponent<Cubic>();

        Vector3 position = _cubic.transform.position;
        _minPositionZ = position.z - _shiftPerMove;
        _maxPositionZ = position.z + _shiftPerMove;

        _canMove = true;
    }

    private void Update()
    {
        _cubic.transform.Translate(Vector3.right * _moveSpeed * Time.deltaTime);
    }

    public void MoveLeft()
    {
        float positionZ = _cubic.transform.position.z;

        if (_canMove && positionZ < _maxPositionZ)
        {
            StartCoroutine(MoveToPositionZ(positionZ + _shiftPerMove));
        }
    }

    public void MoveRight()
    {
        float positionZ = _cubic.transform.position.z;

        if (_canMove && positionZ > _minPositionZ)
        {
            StartCoroutine(MoveToPositionZ(positionZ - _shiftPerMove));
        }
    }

    private IEnumerator MoveToPositionZ(float positionZ)
    {
        _canMove = false;
        yield return _cubic.transform.DOMoveZ(positionZ, _animationSpeed).WaitForCompletion();
        _canMove = true;
    }
}