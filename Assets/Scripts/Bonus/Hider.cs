using System.Collections.Generic;
using UnityEngine;

public class Hider : Bonus
{
    [SerializeField] private Transform _roadsContainer;
    [SerializeField] private Transform _portalsContainer;
    [SerializeField] private LayerMask _hiddenLayer = 1 << 9;
    [SerializeField] private BlockStackRenderer _blockStackRenderer;
    [SerializeField] private bool _isHideCorrectBlocks;

    private Dictionary<ColorBlock, int> _modifiedBlocks;
    private Dictionary<Portal, Color> _modifiedPortals;

    public override void Apply()
    {
        HideUnnecessaryBlocks();
        UpdatePortalColors();
    }

    public override void Cancel()
    {
        ShowAllBlocks();
        RestorePortalColors();
    }

    private void HideUnnecessaryBlocks()
    {
        int newLayer = (int)Mathf.Log(_hiddenLayer.value, 2);

        ColorBlock[] colorBlocks = _roadsContainer.GetComponentsInChildren<ColorBlock>();
        _modifiedBlocks = new Dictionary<ColorBlock, int>(colorBlocks.Length);

        foreach (ColorBlock colorBlock in colorBlocks)
        {
            if (IsInvalidBlockColor(colorBlock.BlockRenderer))
            {
                continue;
            }

            _modifiedBlocks.Add(colorBlock, colorBlock.gameObject.layer);

            colorBlock.gameObject.layer = newLayer;
            colorBlock.BlockPhysics.TurnOffTrigger();
        }
    }

    private bool IsInvalidBlockColor(ColorBlockRenderer blockRenderer)
    {
        Color currentBlockColor = blockRenderer.CurrentColor;
        Color currentCubicColor = _blockStackRenderer.CurrentColor;

        return _isHideCorrectBlocks ? currentBlockColor != currentCubicColor : currentBlockColor == currentCubicColor;
    }

    private void RestorePortalColors()
    {
        foreach ((Portal portal, Color originalColor) in _modifiedPortals)
        {
            portal.SetColor(originalColor);
        }
    }

    private void ShowAllBlocks()
    {
        foreach ((ColorBlock colorBlock, int originalLayer) in _modifiedBlocks)
        {
            colorBlock.gameObject.layer = originalLayer;
            colorBlock.BlockPhysics.TurnOnTrigger();
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