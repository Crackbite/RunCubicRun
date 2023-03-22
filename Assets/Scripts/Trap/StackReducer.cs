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
    [SerializeField] private WidthType _width;
    [SerializeField] private SideType _side;

    public enum SideType
    {
        Left,
        Right
    }

    public enum WidthType
    {
        Short,
        Long
    }

    private void Start()
    {
        SetSize(_height, _width, _side);
    }

    public void SetSize(float height, WidthType width, SideType side)
    {
        Vector3 size = _pillar.localScale;
        _pillar.localScale = new Vector3(size.x, height, size.z);

        Vector3 headPosition = _head.position;
        headPosition.y = _pillar.GetComponent<MeshRenderer>().bounds.size.y + transform.position.y;
        _head.position = headPosition;

        if (width == WidthType.Long)
        {
            _longCrossbar.SetActive(true);
        }
        else if (width == WidthType.Short)
        {
            _shortCrossbar.SetActive(true);
        }

        Vector3 position = transform.position;

        if (side == SideType.Left)
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