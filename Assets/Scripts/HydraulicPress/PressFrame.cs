using UnityEngine;

public class PressFrame : MonoBehaviour
{
    [SerializeField] private PressTopAnimator _pressTopAnimator;
    [SerializeField] private Transform _innerCylinder;

    private void OnEnable()
    {
        _pressTopAnimator.Completed += OnPressTopAnimatorCompleted;
    }

    private void OnDisable()
    {
        _pressTopAnimator.Completed -= OnPressTopAnimatorCompleted;
    }

    private void OnPressTopAnimatorCompleted()
    {
        float highestPoint = _innerCylinder.GetComponent<MeshRenderer>().bounds.max.y;

        Vector3 newPosition = transform.position;
        newPosition.y = highestPoint;

        transform.position = newPosition;
    }
}