using System.Collections.Generic;
using UnityEngine;

public class Colorizer : Bonus
{
    [SerializeField] private ColorBlocksContainer _colorBlocksContainer;
    [SerializeField] private PortalsContainer _portalsContainer;
    [SerializeField] private BlockStackRenderer _blockStackRenderer;

    private Dictionary<ColorBlock, Color> _modifiedBlocks;
    private Dictionary<Portal, Color> _modifiedPortals;

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
        foreach ((Portal portal, Color originalColor) in _modifiedPortals)
        {
            portal.SetColor(originalColor);
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
        _modifiedPortals = new Dictionary<Portal, Color>(portals.Count);

        foreach (Portal portal in portals)
        {
            if (portal.Color == _blockStackRenderer.CurrentColor)
            {
                continue;
            }

            _modifiedPortals.Add(portal, portal.Color);
            portal.SetColor(_blockStackRenderer.CurrentColor);
        }
    }
}