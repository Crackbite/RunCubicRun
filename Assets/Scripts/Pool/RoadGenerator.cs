using UnityEngine;

public class RoadGenerator : ObjectPool
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private Cubic _cubic;
    [SerializeField] private float _offsetX;

    private GameObject _currentRoad;

    private float _minDistance;
    private Vector3 _nextInstallationPosition;
    private Vector3 _roadSize;
    private Vector3 _startPosition;

    private void Awake()
    {
        _startPosition = _prefab.transform.position;

        Initialize(_prefab.gameObject);
        SetRoadToPosition(_startPosition);

        if (_currentRoad == null)
        {
            return;
        }

        _roadSize = Vector3.Scale(
            _currentRoad.transform.localScale,
            _currentRoad.GetComponent<MeshFilter>().mesh.bounds.size);
        _minDistance = _roadSize.x;

        SetInstallationPosition();
    }

    private void Update()
    {
        if (_nextInstallationPosition.x - _cubic.transform.position.x < _minDistance + _offsetX)
        {
            SetRoadToPosition(_nextInstallationPosition);
            _nextInstallationPosition.x += _minDistance;
            DisableObjectAbroadScreen();
        }
    }

    private void SetInstallationPosition()
    {
        Vector3 position = _currentRoad.transform.position;
        _nextInstallationPosition = new Vector3(position.x + _roadSize.x, position.y, position.z);
    }

    private void SetRoadToPosition(Vector3 installationPosition)
    {
        if (TryGetObject(out GameObject item) == false)
        {
            return;
        }

        item.SetActive(true);
        item.transform.position = installationPosition;
        _currentRoad = item;
    }
}