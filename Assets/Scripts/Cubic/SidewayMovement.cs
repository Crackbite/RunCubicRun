using System;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Cubic))]
public class SidewayMovement : MonoBehaviour
{
    [SerializeField] private float _shiftPerMove = 1.3f;
    [SerializeField] private float _changeLineSpeed = .1f;
    [SerializeField] private AudioSource _lineChangingSound;

    private float _currentLineIndex = 1f;
    private float _initialPositionZ;
    private bool _canMove = true;

    private Cubic _cubic;
    private Tweener _lineTweener;

    public event Action LineReached;

    public bool IsInvertControl { get; set; }

    private void Start()
    {
        _cubic = GetComponent<Cubic>();
        _cubic.Hit += OnCubicHit;
        _initialPositionZ = transform.position.z;
    }

    private void OnDisable()
    {
        if (_cubic != null)
        {
            _cubic.Hit -= OnCubicHit;
        }
    }

    public void Move(Vector3 direction)
    {
        const int RightLineIndex = 0;
        const int LeftLineIndex = 2;

        if (IsInvertControl)
        {
            direction *= -1;
        }

        if (IsOnRoad())
        {
            _currentLineIndex += direction.z;
            _currentLineIndex = Mathf.Clamp(_currentLineIndex, RightLineIndex, LeftLineIndex);
            float targetPositionZ = _initialPositionZ + ((_currentLineIndex - 1f) * _shiftPerMove);

            ChangeLine(targetPositionZ);
        }
        else
        {
            LineReached?.Invoke();
        }
    }

    private void ChangeLine(float targetPositionZ)
    {
        _lineTweener = transform.DOMoveZ(targetPositionZ, _changeLineSpeed).OnUpdate(StopLineTweener)
            .OnComplete(() => LineReached?.Invoke());

        if (_cubic.transform.position.z != targetPositionZ)
        {
            _lineChangingSound.Play();
        }
    }

    private bool IsOnRoad()
    {
        const float DistanceToRoad = .5f;

        var ray = new Ray(transform.position, Vector3.down);
        return Physics.Raycast(ray, out RaycastHit _, DistanceToRoad);
    }

    private void StopLineTweener()
    {
        if (_canMove == false)
        {
            _lineTweener.Kill();
        }
    }
    private void OnCubicHit(Vector3 contactPoint, float height)
    {
        _canMove = false;
    }
}