using System.Collections;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Cubic))]
[RequireComponent(typeof(FreeSidewayChecker))]
public class CubicMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _animationSpeed = .1f;
    [SerializeField] private float _shiftPerMove = 1.3f;
    [SerializeField] private AnimationCurve _stopCurve;

    private bool _canMoveToSide;
    private bool _canMoveForward;
    private float _currentSpeed;
    private Cubic _cubic;
    private FreeSidewayChecker _sidewayChacker;
    private float _maxPositionZ;
    private float _minPositionZ;

    private void Awake()
    {
        _cubic = GetComponent<Cubic>();
        _sidewayChacker = GetComponent<FreeSidewayChecker>();
        Vector3 position = _cubic.transform.position;
        _minPositionZ = position.z - _shiftPerMove;
        _maxPositionZ = position.z + _shiftPerMove;

        _canMoveToSide = true;
        _canMoveForward = true;
    }

    private void OnEnable()
    {
        _currentSpeed = _moveSpeed;
        _cubic.Hit += OnHit;
    }

    private void OnDisable()
    {
        _cubic.Hit -= OnHit;
    }

    private void Update()
    {
        if (_canMoveForward)
            _cubic.transform.Translate(Vector3.right * _currentSpeed * Time.deltaTime);
    }

    public void MoveLeft()
    {
        float currentShift = _shiftPerMove;
        Vector3 direction = Vector3.forward;
        float positionZ = _cubic.transform.position.z;

        if (_canMoveToSide && positionZ < _maxPositionZ)
        {
            currentShift = _sidewayChacker.Check(_cubic.transform, currentShift, direction);
            StartCoroutine(MoveToPositionZ(positionZ + currentShift));
        }
    }

    public void MoveRight()
    {
        float currentShift = _shiftPerMove;
        Vector3 direction = Vector3.back;
        float positionZ = _cubic.transform.position.z;

        if (_canMoveToSide && positionZ > _minPositionZ)
        {
            currentShift = _sidewayChacker.Check(_cubic.transform, currentShift, direction);
            StartCoroutine(MoveToPositionZ(positionZ - currentShift));
        }
    }

    private void OnHit()
    {
        _canMoveToSide = false;

        if (_cubic.IsSawing)
            StartCoroutine(StopSlowly());
        else
            _canMoveForward = false;
    }

    private IEnumerator MoveToPositionZ(float positionZ)
    {
        _canMoveToSide = false;
        yield return _cubic.transform.DOMoveZ(positionZ, _animationSpeed).WaitForCompletion();
        _canMoveToSide = true;
    }

    private IEnumerator StopSlowly()
    {
        float runningTime = 0;
        float stopDuration;

        stopDuration = _stopCurve.keys[_stopCurve.length - 1].time;

        while (runningTime <= stopDuration)
        {
            _currentSpeed = _moveSpeed * _stopCurve.Evaluate(runningTime);
            runningTime += Time.deltaTime;
            yield return null;
        }
    }
}