using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject _container;
    [SerializeField] private int _capacity;

    private Camera _camera;

    protected List<GameObject> _pool = new List<GameObject>();

    protected void Initialize(GameObject prefab)
    {
        _camera = Camera.main;

        for (int i = 0; i < _capacity; i++)
        {
            GameObject spawned = Instantiate(prefab, _container.transform);
            spawned.SetActive(false);
            _pool.Add(spawned);
        }
    }

    protected bool TryGetObject(out GameObject result)
    {
        result = _pool.FirstOrDefault(p => p.activeSelf == false);
        return result != null;
    }

    protected void DisableObjectAbroadScreen()
    {
        Vector3 disableLeftPoint = _camera.ViewportToWorldPoint(new Vector3(0, 0.5f, _camera.nearClipPlane));
        Vector3 disableRightPoint = _camera.ViewportToWorldPoint(new Vector3(1, 0.5f, _camera.nearClipPlane));

        foreach (var item in _pool)
        {
            if (item.activeSelf == true)
            {
                if (item.transform.position.x < disableLeftPoint.x)
                {
                    item.SetActive(false);
                }
            }
        }
    }
}
