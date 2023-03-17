using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class PressStand : MonoBehaviour
{
    private MeshRenderer _meshRenderer;

    public Bounds Bounds => _meshRenderer.bounds;

    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }
}