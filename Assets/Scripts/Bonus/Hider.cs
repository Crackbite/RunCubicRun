using System.Collections.Generic;
using UnityEngine;

public class Hider : Bonus
{
    [SerializeField] private ColorBlocksContainer _colorBlocksContainer;
    [SerializeField] private PortalsContainer _portalsContainer;
    [SerializeField] private LayerMask _hiddenLayer = 1 << 9;
    [SerializeField] private BlockStackRenderer _blockStackRenderer;
    [SerializeField] private bool _isHideCorrectBlocks;

    private Dictionary<ColorBlock, int> _modifiedBlocks;

    private void Start()
    {
        AssignComponents();
    }

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

    private void HideUnnecessaryBlocks()
    {
        int newLayer = (int)Mathf.Log(_hiddenLayer.value, 2);

        IReadOnlyList<ColorBlock> colorBlocks = _colorBlocksContainer.ColorBlocks;
        _modifiedBlocks = new Dictionary<ColorBlock, int>(colorBlocks.Count);

        foreach (ColorBlock colorBlock in colorBlocks)
        {
            if (IsInvalidColorBlock(colorBlock))
            {
                continue;
            }

            _modifiedBlocks.Add(colorBlock, colorBlock.gameObject.layer);

            colorBlock.gameObject.layer = newLayer;
            colorBlock.BlockPhysics.TurnOffTrigger();
        }
    }

    private bool IsInvalidColorBlock(ColorBlock colorBlock)
    {
        if (colorBlock == null || colorBlock.IsInStack)
        {
            return true;
        }

        Color currentBlockColor = colorBlock.BlockRenderer.CurrentColor;
        Color currentCubicColor = _blockStackRenderer.CurrentColor;

        return _isHideCorrectBlocks ? currentBlockColor != currentCubicColor : currentBlockColor == currentCubicColor;
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

    private void ShowAllBlocks()
    {
        foreach ((ColorBlock colorBlock, int originalLayer) in _modifiedBlocks)
        {
            if (colorBlock == null)
            {
                continue;
            }

            colorBlock.gameObject.layer = originalLayer;
            colorBlock.BlockPhysics.TurnOnTrigger();
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