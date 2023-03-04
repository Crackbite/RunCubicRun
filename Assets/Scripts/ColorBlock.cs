using UnityEngine;

[RequireComponent(typeof(MeshFilter))]

public class ColorBlock : MonoBehaviour
{
    private Color _color;
    private Vector3 _size;

    public Color CurrentColor => _color;
    public Vector3 Size => _size;

    private void Start()
    {
        _size = Vector3.Scale(transform.localScale, GetComponent<MeshFilter>().mesh.bounds.size);
    }
}
