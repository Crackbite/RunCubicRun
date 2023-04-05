using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class TallTrap : Trap
{
    private MeshRenderer _meshRenderer;
    public Bounds Bounds => _meshRenderer.bounds;

    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }
}
