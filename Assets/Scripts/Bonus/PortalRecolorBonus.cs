using System.Collections.Generic;
using UnityEngine;

public abstract class PortalRecolorBonus : Bonus
{
    [SerializeField] protected ColorBlocksContainer ColorBlocksContainer;
    [SerializeField] protected BlockStackRenderer BlockStackRenderer;
    [SerializeField] protected PortalsContainer PortalsContainer;

    protected void AssignComponents()
    {
        if (ColorBlocksContainer == null)
        {
            ColorBlocksContainer = FindObjectOfType<ColorBlocksContainer>();
        }

        if (PortalsContainer == null)
        {
            PortalsContainer = FindObjectOfType<PortalsContainer>();
        }

        if (BlockStackRenderer == null)
        {
            BlockStackRenderer = FindObjectOfType<BlockStackRenderer>();
        }
    }

    protected void UpdatePortalColors()
    {
        IReadOnlyList<Portal> portals = PortalsContainer.Portals;

        foreach (Portal portal in portals)
        {
            portal.SetColor(BlockStackRenderer.CurrentColor);
        }
    }

    protected void RestorePortalColors()
    {
        IReadOnlyList<Portal> portals = PortalsContainer.Portals;

        foreach (Portal portal in portals)
        {
            portal.TrySetOriginalColor();
        }
    }
}
