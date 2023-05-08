using UnityEngine;

public class CubicStraightFollower : MonoBehaviour
{
    [SerializeField] private Cubic _cubic;
    [SerializeField] private float _xMinLimit = -23f;
    [SerializeField] private float _yLimit = .5f;

    private void Update()
    {
        Vector3 targetPosition = _cubic.transform.position;
        targetPosition.x = Mathf.Max(targetPosition.x, _xMinLimit);
        targetPosition.y = _yLimit;
        targetPosition.z = 0;

        transform.position = targetPosition;
    }
}