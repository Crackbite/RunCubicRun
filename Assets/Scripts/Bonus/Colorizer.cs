using System.Collections.Generic;
using UnityEngine;

public class Colorizer : Bonus
{
    [SerializeField] private Transform _roadsContainer;
    [SerializeField] private Transform _portalsContainer;
    [SerializeField] private BlockStackRenderer _blockStackRenderer;

    private Dictionary<ColorBlock, Color> _modifiedBlocks;
    private Dictionary<Portal, Color> _modifiedPortals;

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
        ColorBlock[] colorBlocks = _roadsContainer.GetComponentsInChildren<ColorBlock>();
        _modifiedBlocks = new Dictionary<ColorBlock, Color>(colorBlocks.Length);

        foreach (ColorBlock colorBlock in colorBlocks)
        {
            if (colorBlock.BlockRenderer.CurrentColor == _blockStackRenderer.CurrentColor)
            {
                continue;
            }

            _modifiedBlocks.Add(colorBlock, colorBlock.BlockRenderer.CurrentColor);
            colorBlock.BlockRenderer.SetColor(_blockStackRenderer.CurrentColor);
        }
    }

    private void UpdatePortalColors()
    {
        Portal[] portals = _portalsContainer.GetComponentsInChildren<Portal>();
        _modifiedPortals = new Dictionary<Portal, Color>(portals.Length);

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