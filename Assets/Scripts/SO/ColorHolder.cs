using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ColorHolder", menuName = "LevelData/ColorHolder", order = 51)]
public class ColorHolder : ScriptableObject
{
    [SerializeField] private Color[] _colors;

    private int _colorCount = 3;

    private void OnValidate()
    {
        if (_colors.Length != _colorCount)
        {
            Array.Resize(ref _colors, _colorCount);
        }
    }

    public IReadOnlyList<Color> Colors => _colors;
}
