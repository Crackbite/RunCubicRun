using DG.Tweening;
using UnityEngine;

public class ColorBlockRenderer : MonoBehaviour
{
    private MeshRenderer _meshRenderer;

    public Color CurrentColor => _meshRenderer.material.color;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    public void SetColor(Color color, int stackPosition = 0, float gradient = 0, float coloringSpeedFactor = 1)
    {
        if (gradient == 0)
        {
            _meshRenderer.material.color = color;
        }
        else
        {
            _meshRenderer.material.DOColor(color, gradient).SetDelay(stackPosition * coloringSpeedFactor);
        }
    }
}