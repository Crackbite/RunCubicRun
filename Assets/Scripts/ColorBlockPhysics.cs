using System;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody), typeof(ColorBlock))]
public class ColorBlockPhysics : MonoBehaviour
{
    [SerializeField] private float _frictionCoefficient = 10f;

    private ColorBlock _colorBlock;
    private Collider _collider;
    private Rigidbody _rigidbody;

    public event Action<int> CrossbarHit;

    private void Awake()
    {
        _colorBlock = GetComponent<ColorBlock>();
        _collider = GetComponent<Collider>();
        _rigidbody = GetComponent<Rigidbody>();
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent(out Crossbar _) == false || _colorBlock.CanFollow == false)
        {
            return;
        }

        collision.isTrigger = false;
        CrossbarHit?.Invoke(_colorBlock.StackPosition);
    }

    public void FallOff(Vector3 fallDirection, float forceFactor = 1)
    {
        const float LifeTime = 5f;

        _rigidbody.isKinematic = false;
        _rigidbody.AddForce(_frictionCoefficient * _colorBlock.StackPosition * forceFactor * fallDirection);
        _collider.isTrigger = false;
        transform.parent = null;
        _colorBlock.StopFollow();
        _colorBlock.enabled = false;
        Destroy(gameObject, LifeTime);
    }
}
