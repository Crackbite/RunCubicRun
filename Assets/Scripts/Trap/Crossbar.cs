using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Crossbar : MonoBehaviour
{
    private Collider _collider;

    public TrapType Type => TrapType.Crossbar;

    private void Start()
    {
        _collider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent(out ColorBlock _))
        {
            _collider.isTrigger = false;
        }
    }
}