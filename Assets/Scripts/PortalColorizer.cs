using System.Collections.Generic;
using UnityEngine;

public class PortalColorizer : MonoBehaviour
{
    [SerializeField] private PortalsContainer _portalsContainer;
    [SerializeField] private ColorHolder _colorHolder;
    [SerializeField] private BlockStackRenderer _blockStackRenderer;

    private Color _previousColor;
    private List<Color> _availableColors;

    private void OnEnable()
    {
        _blockStackRenderer.ColorAssigned += OnStackColorAssigned;
    }

    private void OnDisable()
    {
        _blockStackRenderer.ColorAssigned -= OnStackColorAssigned;
    }

    private void OnStackColorAssigned(Color color)
    {
        _previousColor = color;
        ValidateAvailableColors();
        AssignColorsToPortals();
    }

    private Color ChooseNextColor()
    {
        const float MinValue = -1f;

        float maxValue = _availableColors.Count - 1;
        float random = Random.Range(MinValue, maxValue);

        for (int i = 0; i < _availableColors.Count; i++)
        {
            if (random <= i)
            {
                _previousColor = _availableColors[i];
                ValidateAvailableColors();
                return _previousColor;
            }
        }

        return _previousColor;
    }

    private void ValidateAvailableColors()
    {
        _availableColors = new List<Color>();

        foreach (Color color in _colorHolder.Colors)
        {
            if (color != _previousColor)
            {
                _availableColors.Add(color);
            }
        }
    }

    private void AssignColorsToPortals()
    {
        IReadOnlyList<Portal> portals = _portalsContainer.Portals;

        foreach (Portal portal in portals)
        {
            portal.SetColor(ChooseNextColor());
            portal.CubicEntered += OnCubicEntered;
        }
    }

    private void OnCubicEntered(Portal portal)
    {
        _blockStackRenderer.ChangeColor(portal.Color);
        portal.CubicEntered -= OnCubicEntered;
    }
}
