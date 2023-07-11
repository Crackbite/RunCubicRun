using System.Collections.Generic;
using UnityEngine;

public class Hider : PortalRecolorBonus
{
    [SerializeField] private LayerMask _hiddenLayer = 1 << 9;
    [SerializeField] private LayerMask _defaultLayer = 1 << 3;
    [SerializeField] private bool _isHideCorrectBlocks;

    private List<ColorBlock> _modifiedBlocks;

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

    private void HideUnnecessaryBlocks()
    {
        int newLayer = (int)Mathf.Log(_hiddenLayer.value, 2);

        IReadOnlyList<ColorBlock> colorBlocks = ColorBlocksContainer.ColorBlocks;
        _modifiedBlocks = new List<ColorBlock>(colorBlocks.Count);

        foreach (ColorBlock colorBlock in colorBlocks)
        {
            if (IsInvalidColorBlock(colorBlock))
            {
                continue;
            }

            _modifiedBlocks.Add(colorBlock);

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
        Color currentCubicColor = BlockStackRenderer.CurrentColor;

        return _isHideCorrectBlocks ? currentBlockColor != currentCubicColor : currentBlockColor == currentCubicColor;
    }

    private void ShowAllBlocks()
    {
        int newLayer = (int)Mathf.Log(_defaultLayer.value, 2);

        foreach (ColorBlock colorBlock in _modifiedBlocks)
        {
            if (colorBlock == null)
            {
                continue;
            }

            colorBlock.gameObject.layer = newLayer;
            colorBlock.BlockPhysics.TurnOnTrigger();
        }
    }
}