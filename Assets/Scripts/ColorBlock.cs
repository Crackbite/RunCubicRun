using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class ColorBlock : MonoBehaviour
{
    private MeshRenderer _meshRenderer;

    public Material CurrentMaterial => _meshRenderer.sharedMaterial;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }
}