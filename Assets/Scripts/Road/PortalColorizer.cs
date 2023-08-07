using System.Collections.Generic;
using UnityEngine;

public class PortalColorizer : MonoBehaviour
{
    [SerializeField] private PortalsContainer _portalsContainer;
    [SerializeField] private ColorHolder _colorHolder;
    [SerializeField] private BlockStackRenderer _blockStackRenderer;

    private List<Color> _availableColors;
    private List<Color> _previousColors;
    private float _nextPortalPositionX;

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
        if (_portalsContainer.Portals == null)
        {
            return;
        }

        IReadOnlyList<Portal> portals = _portalsContainer.Portals;
        _previousColors = new List<Color>();

        foreach (Portal portal in _portalsContainer?.Portals)
        {
            Color nextColor;

            if (Mathf.Approximately(portal.transform.position.x, _nextPortalPositionX) == false)
            {
                UpdateAvailableColors();
                _previousColors = new List<Color>();
                _nextPortalPositionX = portal.transform.position.x;
            }

            nextColor = ChooseNextColor();
            portal.SetColor(nextColor);
            portal.CubicEntered += OnCubicEntered;
            _previousColors.Add(nextColor);
            _availableColors.Remove(nextColor);
        }
    }

    private Color ChooseNextColor()
    {
        const int MinValue = 0;

        if (_availableColors.Count == 0)
        {
            UpdateAvailableColors();
        }

        int maxValue = _availableColors.Count;
        int result = Random.Range(MinValue, maxValue);
        return _availableColors[result];
    }

    private void OnCubicEntered(Portal portal)
    {
        _blockStackRenderer.ChangeColor(portal.Color);
    }

    private void OnStackColorAssigned()
    {
        const int FirstPortalIndex = 0;

        if (_portalsContainer.Portals?.Count > 0)
        {
            _nextPortalPositionX = _portalsContainer.Portals[FirstPortalIndex].transform.position.x;
        }

        _previousColors = new List<Color> { _blockStackRenderer.CurrentColor };
        UpdateAvailableColors();
        AssignColorsToPortals();
    }

    private void UpdateAvailableColors()
    {
        _availableColors = new List<Color>();

        foreach (Color color in _colorHolder.Colors)
        {
            if (_previousColors.Contains(color) == false)
            {
                _availableColors.Add(color);
            }
        }
    }
}