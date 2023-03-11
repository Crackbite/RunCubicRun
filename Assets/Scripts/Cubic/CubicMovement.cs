using System.Collections;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Cubic))]
public class CubicMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _animationSpeed = .1f;
    [SerializeField] private float _shiftPerMove = 1.3f;

    private bool _canMoveToSide;
    private bool _canMoveForward;
    private Cubic _cubic;
    private float _maxPositionZ;
    private float _minPositionZ;

    private void Awake()
    {
        _cubic = GetComponent<Cubic>();
        Vector3 position = _cubic.transform.position;
        _minPositionZ = position.z - _shiftPerMove;
        _maxPositionZ = position.z + _shiftPerMove;

        _canMoveToSide = true;
        _canMoveForward = true;
    }

    private void OnEnable()
    {
        _cubic.Hit += OnHit;
    }

    private void OnDisable()
    {
        _cubic.Hit -= OnHit;
    }


    private void Update()
    {
        if (_canMoveForward)
            _cubic.transform.Translate(Vector3.right * _moveSpeed * Time.deltaTime);
    }

    public void MoveLeft()
    {
        float positionZ = _cubic.transform.position.z;

        if (_canMoveToSide && positionZ < _maxPositionZ)
        {
            StartCoroutine(MoveToPositionZ(positionZ + _shiftPerMove));
        }
    }

    public void MoveRight()
    {
        float positionZ = _cubic.transform.position.z;

        if (_canMoveToSide && positionZ > _minPositionZ)
        {
            StartCoroutine(MoveToPositionZ(positionZ - _shiftPerMove));
        }
    }

    private void OnHit()
    {
        _canMoveForward = false;
        _canMoveToSide = false;
    }

    private IEnumerator MoveToPositionZ(float positionZ)
    {
        _canMoveToSide = false;
        yield return _cubic.transform.DOMoveZ(positionZ, _animationSpeed).WaitForCompletion();
        _canMoveToSide = true;
    }
}