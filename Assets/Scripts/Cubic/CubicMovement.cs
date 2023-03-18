using System.Collections;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Cubic), typeof(FreeSidewayChecker))]
public class CubicMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _animationSpeed = .1f;
    [SerializeField] private float _shiftPerMove = 1.3f;
    [SerializeField] private AnimationCurve _stopCurve;

    private bool _canMoveToSide;
    private bool _canMoveForward;
    private bool _isFall;
    private float _currentSpeed;
    private Cubic _cubic;
    private FreeSidewayChecker _sidewayChacker;
    private float _maxPositionZ;
    private float _minPositionZ;
    private float _fallPositionY = -10;

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
        _isFall = _cubic.transform.position.y <= _fallPositionY;

        if (_canMoveForward && _isFall == false)
        {
            _cubic.transform.Translate(Vector3.right * _currentSpeed * Time.deltaTime);
        }
    }

    public void MoveLeft()
    {
        Vector3 direction = Vector3.forward;
        float positionZ = _cubic.transform.position.z;

        if (_canMoveToSide && positionZ < _maxPositionZ)
        {
            StartMoveToSide(positionZ, direction);
        }
    }

    public void MoveRight()
    {
        Vector3 direction = Vector3.back;
        float positionZ = _cubic.transform.position.z;

        if (_canMoveToSide && positionZ > _minPositionZ)
        {
            StartMoveToSide(positionZ, direction);
        }
    }

    private void StartMoveToSide(float positionZ, Vector3 direction)
    {
        float currentShift = _shiftPerMove;

        currentShift = _sidewayChacker.Check(_cubic.transform, currentShift, direction);
        StartCoroutine(MoveToPositionZ(positionZ + currentShift * direction.z));
    }

    private void OnHit()
    {
        _canMoveToSide = false;

        if (_cubic.IsSawing)
        {
            StartCoroutine(StopSlowly());
        }
        else
        {
            _canMoveForward = false;
        }
    }

    private IEnumerator MoveToPositionZ(float positionZ)
    {
        Collider[] colliders = Physics.OverlapSphere(_cubic.transform.position, _cubic.transform.localScale.y);

        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent<Road>(out Road ground))
            {
                break;
            }

            yield break;
        }

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