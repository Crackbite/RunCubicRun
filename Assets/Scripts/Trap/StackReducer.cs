using UnityEngine;

public class StackReducer : MonoBehaviour
{
    [SerializeField] private Transform _pillar;
    [SerializeField] private Transform _head;
    [SerializeField] private GameObject _longCrossbar;
    [SerializeField] private GameObject _shortCrossbar;
    [SerializeField] private float _height;
    [SerializeField] private float _rightPositionZ;
    [SerializeField] private float _leftPositionZ;
    [SerializeField] private bool _isLong;
    [SerializeField] private bool _isLeft;

    private void Start()
    {
        SetSize(_height, _isLong, _isLeft);
    }

    public void SetSize(float height, bool isLong, bool isLeft)
    {
        Vector3 headPosition = _head.position;
        Vector3 size = _pillar.localScale;
        _pillar.localScale = new Vector3(size.x, height, size.z);
        headPosition.y = _pillar.GetComponent<MeshRenderer>().bounds.size.y + transform.position.y;
        _head.position = headPosition;       

        if (isLong)
        {
            _longCrossbar.SetActive(true);
        }
        else
        {
            _shortCrossbar.SetActive(true);
        }

        Vector3 position = transform.position;

        if (isLeft)
        {
            position.z = _leftPositionZ;
            _head.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            position.z = _rightPositionZ;
        }

        transform.position = position;
    }
}
