using System;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody), typeof(ColorBlock))]
public class ColorBlockPhysics : MonoBehaviour
{
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
        if (collision.TryGetComponent(out Crossbar _))
        {
            TurnOffTrigger();
            CrossbarHit?.Invoke(_colorBlock.StackPosition);
        }
    }

    public void FallOff(Vector3 pushForce, float forceFactor, bool removeParent = true)
    {
        _rigidbody.isKinematic = false;
        _rigidbody.AddForce(_colorBlock.StackPosition * forceFactor * pushForce);

        if (removeParent)
        {
            transform.parent = null;
        }

        _colorBlock.StopFollow();
        _colorBlock.enabled = false;
    }

    public void TurnOffTrigger()
    {
        const float MinMass = 0f;

        _collider.isTrigger = false;

        if (_colorBlock.IsInStack == false)
        {
            _rigidbody.mass = MinMass;
            _rigidbody.isKinematic = false;
        }
    }

    public void TurnOnTrigger()
    {
        _collider.isTrigger = true;
        _rigidbody.isKinematic = true;
    }
}