using System;
using UnityEngine;

public class BlockStackRenderer : MonoBehaviour
{
    [SerializeField] private float _gradient = .2f;
    [SerializeField] private float _coloringSpeedFactor = .02f;
    [SerializeField] private BlockStack _blockStack;

    public event Action ColorAssigned;

    public Color CurrentColor { get; private set; }
    public bool IsColorAssigned { get; private set; }

    private void OnEnable()
    {
        _blockStack.BlockAdded += OnBlockAdded;
    }

    private void OnDisable()
    {
        _blockStack.BlockAdded -= OnBlockAdded;
    }

    public void ChangeColor(Color color)
    {
        CurrentColor = color;

        for (int i = 0; i < _blockStack.Blocks.Count; i++)
        {
            int stackPosition = _blockStack.Blocks.Count - i;
            _blockStack.Blocks[i].BlockRenderer.SetColor(color, stackPosition, _gradient, _coloringSpeedFactor);
        }

        TryAssignColor();
    }

    private void OnBlockAdded(ColorBlock colorBlock)
    {
        if (colorBlock.TryGetComponent(out ColorBlockRenderer blockRenderer)
            && blockRenderer.CurrentColor != CurrentColor)
        {
            CurrentColor = blockRenderer.CurrentColor;
        }

        TryAssignColor();
    }

    private void TryAssignColor()
    {
        if (IsColorAssigned == false)
        {
            IsColorAssigned = true;
            ColorAssigned?.Invoke();
        }
    }
}