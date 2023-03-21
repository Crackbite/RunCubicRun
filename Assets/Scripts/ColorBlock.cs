using System;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer), typeof(Collider), typeof(Rigidbody))]
public class ColorBlock : MonoBehaviour
{
    [SerializeField] private float _followSpeed = 350f;
    [SerializeField] private float _frictionCoefficient = 10f;
    [SerializeField] private float _gapSizeFactor = 0.001f;
    [SerializeField] private float _gradient;
    [SerializeField] private float _coloringSpeedFactor = 0.02f;

    private Collider _collider;
    private Cubic _followed;
    private MeshRenderer _meshRenderer;
    private Rigidbody _rigidbody;

    private float _followedDistanceY;
    private float _followedGroundPositionY;
    private float _followedLastPositionY;
    private float _halfSizeDifference;
    private bool _isFallDawn;
    private bool _isGrounded = true;
    private float _lastGroundPositionY;
    private float _runningTime;
    private float _sizeWithGap;
    private float _stackPosition;

    public event Action<int> CrossbarHit;

    public Color CurrentColor => _meshRenderer.material.color;
    public bool IsFollow { get; private set; }

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _collider = GetComponent<Collider>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent(out Crossbar _) == false || IsFollow == false)
        {
            return;
        }

        collision.isTrigger = false;
        CrossbarHit?.Invoke((int)Mathf.Round(_stackPosition));
    }

    private void Update()
    {
        if (IsFollow == false)
        {
            return;
        }

        Vector3 currentPosition = transform.position;

        if (_isGrounded)
        {
            float deltaY = _followed.transform.position.y - _followedLastPositionY;

            if (deltaY > 0)
            {
                _isGrounded = false;
                _lastGroundPositionY = currentPosition.y;
            }
            else if (_followed.transform.position.y < _followedGroundPositionY)
            {
                currentPosition.y = _followed.transform.position.y + _followedDistanceY;
                _isFallDawn = true;
            }
            else if (currentPosition.y != _lastGroundPositionY && _isFallDawn == false)
            {
                _followedDistanceY = currentPosition.y - _followedGroundPositionY;
                _stackPosition = (_followedDistanceY - _halfSizeDifference) / _sizeWithGap;
                _lastGroundPositionY = currentPosition.y;
            }
        }

        Follow(currentPosition);
        _followedLastPositionY = _followed.transform.position.y;
    }

    public void FallOff(Vector3 fallDirection, float forceFactor = 1)
    {
        const float LifeTime = 5f;

        _rigidbody.isKinematic = false;
        _rigidbody.AddForce(_frictionCoefficient * _stackPosition * forceFactor * fallDirection);
        _collider.isTrigger = false;
        IsFollow = false;
        transform.parent = null;
        enabled = false;
        Destroy(gameObject, LifeTime);
    }

    public void PlaceInStack(Cubic followed, float gap)
    {
        _halfSizeDifference = (followed.transform.localScale.y - transform.localScale.y) / 2f;
        _sizeWithGap = transform.localScale.y + gap;
        _lastGroundPositionY = transform.position.y;
        _followedDistanceY = _lastGroundPositionY - _followedGroundPositionY;
        _stackPosition = (_followedDistanceY - _halfSizeDifference) / _sizeWithGap;
        EnableFollow(followed);
    }

    public void SetColor(Color color)
    {
        _meshRenderer.material.DOColor(color, _gradient).SetDelay(_coloringSpeedFactor * _stackPosition);
    }

    private void EnableFollow(Cubic followed)
    {
        _followed = followed;
        IsFollow = true;
        _followedLastPositionY = _followed.transform.position.y;
        _followedGroundPositionY = followed.transform.position.y;
    }

    private void Follow(Vector3 currentPosition)
    {
        float interpolationZ = _followSpeed / _stackPosition * Time.deltaTime;

        if (_isGrounded == false)
        {
            _runningTime += Time.deltaTime;
            float jumpForce = _followed.JumpForce + (_gapSizeFactor * Mathf.Pow(_stackPosition, 2f));
            currentPosition.y = _lastGroundPositionY + jumpForce * _runningTime
                                - _followed.JumpAcceleration * Mathf.Pow(_runningTime, 2f) / 2f;

            if (currentPosition.y < _lastGroundPositionY)
            {
                currentPosition.y = _lastGroundPositionY;
                _runningTime = 0;
                _isGrounded = true;
            }
        }

        transform.position = new Vector3(
            currentPosition.x,
            currentPosition.y,
            Mathf.Lerp(currentPosition.z, _followed.transform.position.z, interpolationZ));
    }
}