using System;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody), typeof(ColorBlock))]
public class ColorBlockPhysics : MonoBehaviour
{
    private Collider _collider;
    private ColorBlock _colorBlock;
    private Rigidbody _rigidbody;

    public event Action<int> CrossbarHit;
    public event Action<ColorBlock> RoadHit;

    public bool IsKnockedByCrossbar { get; private set; }

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
            return;
        }

        if (collision.TryGetComponent(out Road _))
        {
            RoadHit?.Invoke(_colorBlock);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent(out Road _))
        {
            RoadHit?.Invoke(_colorBlock);
        }
    }

    public void FallOff(Vector3 pushForce, float forceFactor, bool removeParent = true, bool isKnockedByCrossbar = false)
    {
        _rigidbody.isKinematic = false;
        _rigidbody.AddForce(_colorBlock.StackPosition * forceFactor * pushForce);

        if (removeParent)
        {
            transform.parent = null;
        }

        if (isKnockedByCrossbar)
        {
            IsKnockedByCrossbar = true;
        }

        _colorBlock.StopFollow();
        _colorBlock.enabled = false;
    }

    public void TurnOffTrigger()
    {
        _collider.isTrigger = false;

        if (_colorBlock.IsInStack == false)
        {
            _rigidbody.isKinematic = false;
        }
    }

    public void TurnOnTrigger()
    {
        _collider.isTrigger = true;
        _rigidbody.isKinematic = true;
    }
}