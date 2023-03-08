using System.Collections;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Cubic))]
public class CubicMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _changeLineSpeed = .1f;
    [SerializeField] private float _shiftPerMove = 1.3f;
    [SerializeField] private float _stopAtPressStandSpeed = 1f;

    private bool _canLineChange = true;
    private bool _canMove = true;

    private Cubic _cubic;

    private float _maxPositionZ;
    private float _minPositionZ;

    private void Awake()
    {
        _cubic = GetComponent<Cubic>();

        Vector3 position = _cubic.transform.position;
        _minPositionZ = position.z - _shiftPerMove;
        _maxPositionZ = position.z + _shiftPerMove;
    }

    private void OnEnable()
    {
        _cubic.SteppedOnStand += CubicOnSteppedOnStand;
    }

    private void Update()
    {
        if (_canMove)
        {
            _cubic.transform.Translate(_moveSpeed * Time.deltaTime * Vector3.right);
        }
    }

    private void OnDisable()
    {
        _cubic.SteppedOnStand -= CubicOnSteppedOnStand;
    }

    public void MoveLeft()
    {
        float positionZ = _cubic.transform.position.z;

        if (_canLineChange && positionZ < _maxPositionZ)
        {
            StartCoroutine(MoveToPositionZ(positionZ + _shiftPerMove));
        }
    }

    public void MoveRight()
    {
        float positionZ = _cubic.transform.position.z;

        if (_canLineChange && positionZ > _minPositionZ)
        {
            StartCoroutine(MoveToPositionZ(positionZ - _shiftPerMove));
        }
    }

    private void CubicOnSteppedOnStand(PressStand pressStand)
    {
        _canMove = false;
        _canLineChange = false;

        Vector3 standCenter = pressStand.GetComponent<Collider>().bounds.center;
        var nextPosition = new Vector3(standCenter.x, _cubic.transform.position.y, standCenter.z);

        _cubic.transform.DOMove(nextPosition, _stopAtPressStandSpeed);
    }

    private IEnumerator MoveToPositionZ(float positionZ)
    {
        _canLineChange = false;
        yield return _cubic.transform.DOMoveZ(positionZ, _changeLineSpeed).WaitForCompletion();
        _canLineChange = true;
    }
}