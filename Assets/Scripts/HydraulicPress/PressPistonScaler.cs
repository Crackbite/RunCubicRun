using UnityEngine;

public class PressPistonScaler : MonoBehaviour
{
    [SerializeField] private PressTopAnimator _pressTopAnimator;
    [SerializeField] private float _scaleFactor = 1.6f;

    private Vector3 _initialPosition;
    private float _initialScaleY;
    private bool _isWorking;

    private void OnEnable()
    {
        _pressTopAnimator.Completed += OnPressTopAnimatorCompleted;
    }

    private void Update()
    {
        if (_isWorking == false)
        {
            return;
        }

        Vector3 localScale = transform.localScale;
        localScale.y = _initialScaleY + ((_initialPosition.y - transform.position.y) * _scaleFactor);

        transform.localScale = localScale;
    }

    private void OnDisable()
    {
        _pressTopAnimator.Completed -= OnPressTopAnimatorCompleted;
    }

    private void OnPressTopAnimatorCompleted()
    {
        _initialPosition = transform.position;
        _initialScaleY = transform.localScale.y;

        _isWorking = true;
    }
}