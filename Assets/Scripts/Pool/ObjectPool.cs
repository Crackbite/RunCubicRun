using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject _container;
    [SerializeField] private int _capacity;

    protected List<GameObject> _pool = new();

    private Camera _camera;

    protected void DisableObjectAbroadScreen()
    {
        Vector3 disableLeftPoint = _camera.ViewportToWorldPoint(new Vector3(0, 0.5f, _camera.nearClipPlane));

        foreach (GameObject item in _pool)
        {
            if (item.activeSelf == false)
            {
                continue;
            }

            if (item.transform.position.x < disableLeftPoint.x)
            {
                item.SetActive(false);
            }
        }
    }

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
}