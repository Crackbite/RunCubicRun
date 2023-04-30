using System.Collections.Generic;
using UnityEngine;

public class PortalColorizer : MonoBehaviour
{
    [SerializeField] private PortalsContainer _portalsContainer;
    [SerializeField] private ColorHolder _colorHolder;
    [SerializeField] private BlockStackRenderer _blockStackRenderer;

    private List<Color> _availableColors;
    private float _nextPortalPositionX;

    private void OnEnable()
    {
        _blockStackRenderer.ColorAssigned += OnStackColorAssigned;
    }

    private void OnDisable()
    {
        _blockStackRenderer.ColorAssigned -= OnStackColorAssigned;
    }

    private void OnStackColorAssigned()
    {
        const int FirstPortalIndex = 0;

        _nextPortalPositionX = _portalsContainer.Portals[FirstPortalIndex].transform.position.x;
        ValidateAvailableColors();
        AssignColorsToPortals();
    }

    private Color ChooseNextColor()
    {
        const float MinValue = -1f;

        if (_availableColors.Count > 0)
        {
            float maxValue = _availableColors.Count - 1;
            float random = Random.Range(MinValue, maxValue);

            for (int i = 0; i < _availableColors.Count; i++)
            {
                if (random <= i)
                {
                    return _availableColors[i];
                }
            }
        }

        return _blockStackRenderer.CurrentColor;
    }

    private void ValidateAvailableColors()
    {
        _availableColors = new List<Color>();

        foreach (Color color in _colorHolder.Colors)
        {
            if (color != _blockStackRenderer.CurrentColor)
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
            if (portal.IsColored == false)
            {
                if(portal.transform.position.x == _nextPortalPositionX)
                {
                    Color nextColor = ChooseNextColor();
                    portal.SetColor(nextColor);
                    portal.CubicEntered += OnCubicEntered;
                    _availableColors.Remove(nextColor);
                }
                else
                {
                    _nextPortalPositionX = portal.transform.position.x;
                    return;
                }
            }
        }
    }

    private void OnCubicEntered(Portal portal)
    {
        _blockStackRenderer.ChangeColor(portal.Color);
        ValidateAvailableColors();
        AssignColorsToPortals();
    }
}
