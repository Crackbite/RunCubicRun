using System.Collections.Generic;
using UnityEngine;

public class Colorizer : PortalRecolorBonus
{
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

    private void UpdateBlockColors()
    {
        IReadOnlyList<ColorBlock> colorBlocks = ColorBlocksContainer.ColorBlocks;
        _modifiedBlocks = new Dictionary<ColorBlock, Color>(colorBlocks.Count);

        foreach (ColorBlock colorBlock in colorBlocks)
        {
            if (colorBlock == null || colorBlock.BlockRenderer.CurrentColor == BlockStackRenderer.CurrentColor)
            {
                continue;
            }

            _modifiedBlocks.Add(colorBlock, colorBlock.BlockRenderer.CurrentColor);
            colorBlock.BlockRenderer.SetColor(BlockStackRenderer.CurrentColor);
        }
    }
}