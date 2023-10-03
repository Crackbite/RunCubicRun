using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Crossbar : MonoBehaviour
{
    [SerializeField] private ParticleSystem _collisionEffect;
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
            Vector3 contactPoint = _collider.ClosestPoint(collision.transform.position);
            _collisionEffect.transform.position = contactPoint;
            _collisionEffect.Play();
        }
    }
}