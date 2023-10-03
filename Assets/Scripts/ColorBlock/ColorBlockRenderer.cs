using DG.Tweening;
using UnityEngine;

public class ColorBlockRenderer : MonoBehaviour
{
    private MeshRenderer _meshRenderer;
    private readonly string ColorName = "_EmissionColor";

    public Color CurrentColor => _meshRenderer.material.GetColor(ColorName);

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    public void SetColor(Color color, int stackPosition = 0, float gradient = 0f, float coloringSpeedFactor = 1f)
    {
        if (gradient == 0f)
        {
            _meshRenderer.material.SetColor(ColorName, color);
        }
        else
        {
            _meshRenderer.material.DOColor(color, ColorName, gradient).SetDelay(stackPosition * coloringSpeedFactor);
        }
    }
}