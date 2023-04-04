using System;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Cubic))]
public class SidewayMovement : MonoBehaviour
{
    [SerializeField] private LayerMask _ignoreLayerMask = 1 << 7;
    [SerializeField] private float _shiftPerMove = 1.3f;
    [SerializeField] private float _changeLineSpeed = .1f;

    private float _currentLineIndex = 1f;
    private float _initialPositionZ;
    private Cubic _cubic;

    private Tweener _lineTweener;

    public event Action LineReached;

    private void Start()
    {
        _cubic = GetComponent<Cubic>();
        _initialPositionZ = transform.position.z;
    }

    public void Move(Vector3 direction)
    {
        const int RightLineIndex = 0;
        const int LeftLineIndex = 2;

        if (IsOnRoad())
        {
            _currentLineIndex += direction.z;
            _currentLineIndex = Mathf.Clamp(_currentLineIndex, RightLineIndex, LeftLineIndex);
            float targetPositionZ = _initialPositionZ + ((_currentLineIndex - 1) * _shiftPerMove);

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
    }

    private bool IsOnRoad()
    {
        const float DistanceToRoad = 0.5f;

        var ray = new Ray(transform.position, Vector3.down);
        return Physics.Raycast(ray, out RaycastHit _, DistanceToRoad);
    }

    private bool IsWayClear()
    {
        const int MaxColliders = 5;
        const float ColliderOffset = 0.1f;

        float boxSize = (transform.localScale.z / 2f) - ColliderOffset;
        var colliders = new Collider[MaxColliders];
        var area = new Vector3(boxSize, boxSize, boxSize);

        int hitCount = Physics.OverlapBoxNonAlloc(
            transform.position,
            area,
            colliders,
            transform.rotation,
            _ignoreLayerMask);

        return hitCount > 0;
    }

    private void StopLineTweener()
    {
        if (IsWayClear() && _cubic.CanDestroy == false)
        {
            _lineTweener.Kill();
        }
    }
}