using UnityEngine;

public class CubicTracker : MonoBehaviour
{
    [SerializeField] private Transform _cubic;
    [SerializeField] private float _damping;
    [SerializeField] private float _xOffset;
    [SerializeField] private float _xLeftLimit;
    [SerializeField] private float _xRightLimit;

    private Vector3 _targetPosition;

    private void LateUpdate()
    {
        SetTargetPosition();
        transform.position = Vector3.Lerp(transform.position, _targetPosition, _damping * Time.deltaTime);
    }

    private void SetTargetPosition()
    {
        _targetPosition = new Vector3(_cubic.position.x + _xOffset, transform.position.y, transform.position.z);
        _targetPosition.x = Mathf.Clamp(_targetPosition.x, _xLeftLimit, _xRightLimit + _cubic.transform.position.x);
    }
}
