using System;
using UnityEngine;

public class CubicTracker : MonoBehaviour
{
    [SerializeField] private Cubic _cubic;
    [SerializeField] private float _damping;
    [SerializeField] private float _xOffset;
    [SerializeField] private float _xLeftLimit;
    [SerializeField] private float _xRightLimit;
    [SerializeField] private LevelEntryPortal _levelEntryPortal;

    private Vector3 _targetPosition;
    bool _canTrack;

    private void OnEnable()
    {
        _levelEntryPortal.ThrowingOut += OnCubicThrownOut;
    }

    private void LateUpdate()
    {
        if (_canTrack)
        {
            SetTargetPosition();
            transform.position = Vector3.Lerp(transform.position, _targetPosition, _damping * Time.deltaTime);
        }
    }

    private void OnDisable()
    {
        _levelEntryPortal.ThrowingOut -= OnCubicThrownOut;
    }

    private void OnCubicThrownOut()
    {
        _canTrack = true;
    }

    private void SetTargetPosition()
    {
        Vector3 trackerPosition = transform.position;
        Vector3 cubicPosition = _cubic.transform.position;

        _targetPosition = new Vector3(cubicPosition.x + _xOffset, trackerPosition.y, trackerPosition.z);
        _targetPosition.x = Mathf.Clamp(_targetPosition.x, _xLeftLimit, _xRightLimit);
    }
}