using System.Collections.Generic;
using UnityEngine;

public class PortalColorizer : MonoBehaviour
{
    [SerializeField] private PortalsContainer _portalsContainer;
    [SerializeField] private ColorHolder _colorHolder;
    [SerializeField] private BlockStackRenderer _blockStackRenderer;

    private void OnEnable()
    {
        _blockStackRenderer.ColorAssigned += OnStackColorAssigned;
    }

    private void OnDisable()
    {
        _blockStackRenderer.ColorAssigned -= OnStackColorAssigned;
    }

    private void AssignColorsToPortals()
    {
        IReadOnlyList<Portal> portals = _portalsContainer.Portals;
        int currentColorIndex = GetNextColorIndex(_blockStackRenderer.CurrentColor);
        int previousColorIndex = currentColorIndex;

        foreach (Portal portal in portals)
        {
            portal.SetColor(_colorHolder.Colors[currentColorIndex]);
            portal.CubicEntered += OnCubicEntered;
            currentColorIndex = GetNextColorIndex(_colorHolder.Colors[previousColorIndex]);
            previousColorIndex = currentColorIndex;
        }
    }

    private int GetNextColorIndex(Color exceptColor)
    {
        const int MinValue = 0; 

        int maxValue = _colorHolder.Colors.Count;
        int result = Random.Range(MinValue, maxValue);

        while (_colorHolder.Colors[result] == exceptColor)
        {
            result = Random.Range(MinValue, maxValue);
        }

        return result;
    }

    private void OnCubicEntered(Portal portal)
    {
        _blockStackRenderer.ChangeColor(portal.Color);
        portal.CubicEntered -= OnCubicEntered;
    }

    private void OnStackColorAssigned()
    {
        AssignColorsToPortals();
    }
}