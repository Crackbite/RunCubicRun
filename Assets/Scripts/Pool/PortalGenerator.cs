using System.Collections.Generic;
using UnityEngine;

public class PortalGenerator : ObjectPool
{
    [SerializeField] private GameObject _template;
    [SerializeField] private Cubic _cubic;
    [SerializeField] private ColorHolder _colorHolder;
    [SerializeField] private BlockStackRenderer _blockStackRenderer;
    [SerializeField] private List<Transform> _instalationPoints;

    private List<Color> _unusedColors;
    private Color _usedColor;

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
        Initialize(_template);
        _usedColor = color;
        SortUnusedColors();

        if (_instalationPoints.Count > 0)
        {
            SetToPosition(GetInstallationPosition());
        }
    }

    private Vector3 GetInstallationPosition()
    {
        int firstPointIndex = 0;
        Vector3 position = _instalationPoints[firstPointIndex].position;
        _instalationPoints.RemoveAt(firstPointIndex);
        return position;
    }

    private void OnCubicEntered(Portal portal)
    {
        _blockStackRenderer.ChangeColor(portal.Color);
        portal.CubicEntered -= OnCubicEntered;
        DisableObjectAbroadScreen();

        if (_instalationPoints.Count > 0)
        {
            SetToPosition(GetInstallationPosition());
        }
    }

    private Color SetNextColor()
    {
        const float MinValue = -1f;

        float maxValue = _unusedColors.Count - 1;
        float random = Random.Range(MinValue, maxValue);

        for (int i = 0; i < _unusedColors.Count; i++)
        {
            if (random <= i)
            {
                _usedColor = _unusedColors[i];
                SortUnusedColors();
                return _usedColor;
            }
        }

        return _usedColor;
    }

    private void SetToPosition(Vector3 installationPosition)
    {
        if (TryGetObject(out GameObject item) == false || item.TryGetComponent(out Portal portal) == false)
        {
            return;
        }

        item.SetActive(true);
        portal.transform.position = installationPosition;
        portal.SetColor(SetNextColor());
        portal.CubicEntered += OnCubicEntered;
    }

    private void SortUnusedColors()
    {
        _unusedColors = new List<Color>();

        foreach (Color color in _colorHolder.Colors)
        {
            if (color != _usedColor)
            {
                _unusedColors.Add(color);
            }
        }
    }
}