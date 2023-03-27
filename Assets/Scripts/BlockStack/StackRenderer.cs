using UnityEngine;

public class StackRenderer : MonoBehaviour
{
    [SerializeField] private float _gradient = .2f;
    [SerializeField] private float _coloringSpeedFactor = .02f;
    [SerializeField] private ColorBlockCollection _blockCollection;

    public Color CurrentColor { get; private set; }

    private void OnEnable()
    {
        _blockCollection.BlockAdded += OnBlockAdded;
    }

    private void OnDisable()
    {
        _blockCollection.BlockAdded -= OnBlockAdded;
    }

    public void ChangeColor(Color color)
    {
        CurrentColor = color;

        for (int i = 0; i < _blockCollection.Blocks.Count; i++)
        {
            int stackPosition = _blockCollection.Blocks.Count - i;
            _blockCollection.Blocks[i].BlockRenderer.SetColor(color, stackPosition, _gradient, _coloringSpeedFactor);
        }
    }

    private void OnBlockAdded(ColorBlock colorBlock)
    {
        if (colorBlock.TryGetComponent(out ColorBlockRenderer blockRenderer)
            && blockRenderer.CurrentColor != CurrentColor)
        {
            CurrentColor = blockRenderer.CurrentColor;
        }
    }
}