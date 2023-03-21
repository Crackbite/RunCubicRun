using System.Collections.Generic;
using UnityEngine;

public class PortalGenerator : ObjectPool
{
    [SerializeField] private GameObject _template;
    [SerializeField] private Cubic _cubic;
    [SerializeField] private ColorHolder _colorHolder;
    [SerializeField] private BlocksContainer _blockContainer;
    [SerializeField] private float _skipDistance;

    private List<Color> _unusedColors;
    private Color _usedColor;

    private void Start()
    {
        Initialize(_template);
        SortUnusedColors();
        SetToPosition(GetInstallationPosition());
    }

    private Vector3 GetInstallationPosition()
    {
        Vector3 position = _template.transform.position;
        return new Vector3(_cubic.transform.position.x + _skipDistance, position.y, position.z);
    }

    private void OnCubicEntered(Portal portal)
    {
        _blockContainer.ChangeColor(portal.Color);
        SetToPosition(GetInstallationPosition());
        portal.CubicEntered -= OnCubicEntered;
        DisableObjectAbroadScreen();
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