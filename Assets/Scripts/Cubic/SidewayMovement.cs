using DG.Tweening;
using System;
using UnityEngine;

public class SidewayMovement : MonoBehaviour
{
    [SerializeField] private LayerMask _ignoreLayerMask = 1 << 7;
    [SerializeField] private float _shiftPerMove = 1.3f;
    [SerializeField] private float _changeLineSpeed = .1f;

    private float _currentLine = 1;
    private float _startPosition;

    public event Action LineReached;

    private void Start()
    {
        _startPosition = transform.position.z;
    }

    public void Move(Vector3 direction)
    {
        const int RightLineIndex = 0;
        const int LeftLineIndex = 2;

        if (IsOnRoad())
        {
            _currentLine += direction.z;
            _currentLine = Mathf.Clamp(_currentLine, RightLineIndex, LeftLineIndex);
            float cubicPositionZ = _startPosition + (_currentLine - 1) * _shiftPerMove;
            ChangeLine(cubicPositionZ);
        }
        else
        {
            LineReached?.Invoke();
        }
    }

    private void ChangeLine(float cubicPositionZ)
    {
        Tweener tween = null;

        tween = transform.DOMoveZ(cubicPositionZ, _changeLineSpeed).OnUpdate(() =>
        {
            if(CheckFreeWay())
            {
                tween.Kill();
            }
        }).OnComplete(() => LineReached?.Invoke());
    }

    private bool IsOnRoad()
    {
        const float DistanceToRoad = 0.5f;

        RaycastHit hit;
        var ray = new Ray(transform.position, Vector3.down);
        Debug.Log(Physics.Raycast(ray, out hit, DistanceToRoad));
        if (Physics.Raycast(ray, out hit, DistanceToRoad))
        {
            return true;
        }

        return false;
    }

    public bool CheckFreeWay()
    {
        const int MaxAmount = 5;
        const float OffSet = 0.1f;

        float size = transform.localScale.z / 2f - OffSet;        
        var colliders = new Collider[MaxAmount];
        var area = new Vector3(size, size, size);
        var hitCount = Physics.OverlapBoxNonAlloc(transform.position, area, colliders, transform.rotation, _ignoreLayerMask);

        if (hitCount > 0)
        {
            return true;
        }

        return false;
    }
}