using DG.Tweening;
using System;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer), typeof(Collider), typeof(Rigidbody))]
public class ColorBlock : MonoBehaviour
{
    [SerializeField] private float _followSpeed = 350f;
    [SerializeField] private float _frictionCoefficient = 10f;
    [SerializeField] private float _gapSizeFactor = 0.001f;
    [SerializeField] private float _gradient = 0f;
    [SerializeField] private float _coloringSpeedFactor = 0.02f;

    private bool _isFollow;
    private bool _isGrounded = true;
    private bool _isFallDawn;
    private Cubic _followed;
    private MeshRenderer _meshRenderer;
    private Collider _collider;
    private Rigidbody _rigidbody;
    private float _stackPosition;
    private float _halfSizeDifference;
    private float _sizeWithGap;
    private float _followedLastPositionY;
    private float _followedGroundPositionY;
    private float _lastGroundPositionY;
    private float _runningTime;
    float _followedDistanceY;

    public bool IsFollow => _isFollow;
    public Color CurrentColor => _meshRenderer.material.color;

    public event Action<int> CrossbarHit;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _collider = GetComponent<Collider>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (_isFollow == false)
        {
            return;
        }

        Vector3 currentPosition = transform.position;
        float deltaY = _followed.transform.position.y - _followedLastPositionY;

        if (_isGrounded)
        {
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

    private void Follow(Vector3 currentPosition)
    {
        float interpolationZ = _followSpeed / _stackPosition * Time.deltaTime;

        if (_isGrounded == false)
        {
            _runningTime += Time.deltaTime;
            float jumpForce = _followed.JumpForce + _gapSizeFactor * Mathf.Pow(_stackPosition, 2);
            currentPosition.y = _lastGroundPositionY + jumpForce * _runningTime - _followed.JumpAcceleration * Mathf.Pow(_runningTime, 2) / 2;

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

    public void SetColor(Color color)
    {
        _meshRenderer.material.DOColor(color, _gradient).SetDelay(_coloringSpeedFactor * _stackPosition);
    }

    public void PlaceInStack(Cubic followed, float gap)
    {
        _halfSizeDifference = (followed.transform.localScale.y - transform.localScale.y) / 2;
        _sizeWithGap = transform.localScale.y + gap;
        _lastGroundPositionY = transform.position.y;
        _followedDistanceY = _lastGroundPositionY - _followedGroundPositionY;
        _stackPosition = (_followedDistanceY - _halfSizeDifference) / _sizeWithGap;
        EnableFollow(followed);
    }


    public void FallOff(Vector3 fallDirection, float forceFactor = 1)
    {
        float lifeTime = 5f;

        _rigidbody.isKinematic = false;
        _rigidbody.AddForce(fallDirection * forceFactor *_frictionCoefficient * _stackPosition);
        _collider.isTrigger = false;
        _isFollow = false;
        transform.parent = null;
        enabled = false;
        Destroy(gameObject, lifeTime);
    }

    private void EnableFollow(Cubic followed)
    {
        _followed = followed;
        _isFollow = true;
        _followedLastPositionY = _followed.transform.position.y;
        _followedGroundPositionY = followed.transform.position.y;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.TryGetComponent(out Crossbar _) == false || _isFollow == false)
        {
            return;
        }

        collision.isTrigger = false;
        CrossbarHit?.Invoke((int)Mathf.Round(_stackPosition));
    }
}