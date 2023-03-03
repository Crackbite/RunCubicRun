using UnityEngine;
using UnityEngine.EventSystems;

public class CubicMover : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [SerializeField] private Cubic _cubic;
    [SerializeField] private float _speed;
    [SerializeField] private float _shiftPerSwipe;
    [SerializeField] private float _sideShiftSpeed;

    private float _leftPositionZ;
    private float _centerPositionZ;
    private float _rightPositionZ;
    private bool _canMoveToSide = true;

    private void Start()
    {
        _leftPositionZ = _cubic.transform.position.z + _shiftPerSwipe;
        _centerPositionZ = _cubic.transform.position.z;
        _rightPositionZ = _cubic.transform.position.z - _shiftPerSwipe;
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        _cubic.transform.Translate(Vector3.right * _speed * Time.deltaTime);
    }

    public void OnDrag(PointerEventData eventData)
    {
        int neutralDelta = 0;

        if(_canMoveToSide) 
        {
            if (eventData.delta.y > neutralDelta && _cubic.transform.position.z < _leftPositionZ)
            {
                if (_cubic.transform.position.z < _centerPositionZ)
                    MoveToSide(_centerPositionZ);
                else
                    MoveToSide(_leftPositionZ);
            }
            else if (eventData.delta.y < neutralDelta && _cubic.transform.position.z > _rightPositionZ)
            {
                if (_cubic.transform.position.z > _centerPositionZ)
                    MoveToSide(_centerPositionZ);
                else
                    MoveToSide(_rightPositionZ);
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _canMoveToSide = true;
    }

    private void MoveToSide(float positionZ)
    {
        _cubic.transform.position = new Vector3(_cubic.transform.position.x, _cubic.transform.position.y, positionZ);
        _canMoveToSide = false;
    }
}
