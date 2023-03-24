using System.Collections.Generic;
using UnityEngine;

public class StackRenderer : MonoBehaviour
{
    [SerializeField] private BlockStacker _blockStacker;
    [SerializeField] private BlocksContainer _blocksContainer;
    [SerializeField] private float _gradient = .2f;
    [SerializeField] private float _coloringSpeedFactor = .02f;

    private readonly List<ColorBlockRenderer> _blockRenderers = new();

    public Color CurrentColor { get; private set; }

    private void OnEnable()
    {
        _blockStacker.ColorBlockAdded += OnColorBlockAdded;
    }

    private void OnDisable()
    {
        _blockStacker.ColorBlockAdded -= OnColorBlockAdded;
    }

    public void ChangeColor(Color color)
    {
        CurrentColor = color;

        for (int i = 0; i < _blockRenderers.Count; i++)
        {
            int stackPosition = _blockRenderers.Count - i;
            _blockRenderers[i].SetColor(color,stackPosition, _gradient, _coloringSpeedFactor);
        }
    }

    private void OnColorBlockAdded(ColorBlock colorBlock)
    {
        if(colorBlock.TryGetComponent(out ColorBlockRenderer blockRenderer) && blockRenderer.CurrentColor != CurrentColor)
        {
            CurrentColor = blockRenderer.CurrentColor;
        }

        _blockRenderers.Add(blockRenderer);
    }
}
