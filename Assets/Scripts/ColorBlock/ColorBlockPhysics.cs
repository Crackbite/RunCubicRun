using System;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody), typeof(ColorBlock))]
public class ColorBlockPhysics : MonoBehaviour
{
    private const float DefaultForceFactor = 1f;

    private Collider _collider;
    private ColorBlock _colorBlock;
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

    public void FallOff(Vector3 fallDirection, float frictionCoefficient, float forceFactor = DefaultForceFactor)
    {
        _rigidbody.isKinematic = false;
        _rigidbody.AddForce(frictionCoefficient * _colorBlock.StackPosition * forceFactor * fallDirection);

        if (Math.Abs(forceFactor - DefaultForceFactor) != 0)
        {
            transform.parent = null;
        }

        _colorBlock.StopFollow();
        _colorBlock.enabled = false;
    }

    public void TurnOffTrigger()
    {
        _collider.isTrigger = false;
    }
}