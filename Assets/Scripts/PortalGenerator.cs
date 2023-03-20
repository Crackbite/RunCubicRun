using System.Collections.Generic;
using UnityEngine;

public class PortalGenerator : ObjectPool
{
    [SerializeField] private GameObject _template;
    [SerializeField] private Cubic _cubic;
    [SerializeField] private ColorHolder _colorHolder;
    [SerializeField] private BlocksContainer _blockContainer;
    [SerializeField] private float _skipDistance;

    private Color _usedColor;
    private List<Color> _unusedColors;

    private void Start()
    {
        Initialize(_template.gameObject);
        SortUnusedColors();
        SetToPosition(GetInstallationPosition());
    }

    private Color SetNextColor()
    {
        float minValue = -1f;
        float maxValue = _unusedColors.Count - 1;

        float random = Random.Range(minValue, maxValue);

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

    private void SortUnusedColors()
    {
        _unusedColors = new List<Color>();

        foreach (var color in _colorHolder.Colors)
        {
            if (color != _usedColor)
                _unusedColors.Add(color);
        }
    }

    private void SetToPosition(Vector3 installationPosition)
    {
        if (TryGetObject(out GameObject item))
        {
            if (item.TryGetComponent<Portal>(out Portal portal))
            {
                item.SetActive(true);
                portal.transform.position = installationPosition;
                portal.SetColor(SetNextColor());
                portal.CubicEntered += OnCubicEntered;
            }
        }
    }

    private Vector3 GetInstallationPosition()
    {
        return new Vector3(_cubic.transform.position.x + _skipDistance, _template.transform.position.y, _template.transform.position.z);
    }

    private void OnCubicEntered(Portal portal)
    {
        _blockContainer.ChangeColor(portal.Color);
        SetToPosition(GetInstallationPosition());
        portal.CubicEntered -= OnCubicEntered;
        DisableObjectAbroadScreen();
    }
}