using System.Collections.Generic;
using UnityEngine;

public class Colorizer : Bonus
{
    [SerializeField] private ColorBlocksContainer _colorBlocksContainer;
    [SerializeField] private PortalsContainer _portalsContainer;
    [SerializeField] private BlockStackRenderer _blockStackRenderer;

    private Dictionary<ColorBlock, Color> _modifiedBlocks;

    private void Start()
    {
        AssignComponents();
    }

    public override void Apply()
    {
        UpdateBlockColors();
        UpdatePortalColors();
    }

    public override void Cancel()
    {
        RestoreBlockColors();
        RestorePortalColors();
    }

    private void AssignComponents()
    {
        if (_colorBlocksContainer == null)
        {
            _colorBlocksContainer = FindObjectOfType<ColorBlocksContainer>();
        }

        if (_portalsContainer == null)
        {
            _portalsContainer = FindObjectOfType<PortalsContainer>();
        }

        if (_blockStackRenderer == null)
        {
            _blockStackRenderer = FindObjectOfType<BlockStackRenderer>();
        }
    }

    private void RestoreBlockColors()
    {
        foreach ((ColorBlock colorBlock, Color originalColor) in _modifiedBlocks)
        {
            if (colorBlock == null || colorBlock.IsInStack)
            {
                continue;
            }

            colorBlock.BlockRenderer.SetColor(originalColor);
        }
    }

    private void RestorePortalColors()
    {
        IReadOnlyList<Portal> portals = _portalsContainer.Portals;

        foreach (Portal portal in portals)
        {
            if (portal.UsedByBonus != this)
            {
                continue;
            }

            portal.SetOriginalColor();
            portal.UsedByBonus = null;
        }
    }

    private void UpdateBlockColors()
    {
        IReadOnlyList<ColorBlock> colorBlocks = _colorBlocksContainer.ColorBlocks;
        _modifiedBlocks = new Dictionary<ColorBlock, Color>(colorBlocks.Count);

        foreach (ColorBlock colorBlock in colorBlocks)
        {
            if (colorBlock == null || colorBlock.BlockRenderer.CurrentColor == _blockStackRenderer.CurrentColor)
            {
                continue;
            }

            _modifiedBlocks.Add(colorBlock, colorBlock.BlockRenderer.CurrentColor);
            colorBlock.BlockRenderer.SetColor(_blockStackRenderer.CurrentColor);
        }
    }

    private void UpdatePortalColors()
    {
        IReadOnlyList<Portal> portals = _portalsContainer.Portals;

        foreach (Portal portal in portals)
        {
            portal.SetColor(_blockStackRenderer.CurrentColor);
            portal.UsedByBonus = this;
        }
    }
}