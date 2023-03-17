using System;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class Cubic : MonoBehaviour
{
    private MeshRenderer _meshRenderer;
    private bool _steppedOnStand;

    public event Action<PressStand> SteppedOnStand;

    public Bounds Bounds => _meshRenderer.bounds;

    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (_steppedOnStand || collision.TryGetComponent(out PressStand pressStand) == false)
        {
            return;
        }

        _steppedOnStand = true;
        SteppedOnStand?.Invoke(pressStand);
    }
}