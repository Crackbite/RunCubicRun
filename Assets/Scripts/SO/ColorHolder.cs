using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ColorHolder", menuName = "LevelData/ColorHolder", order = 51)]
public class ColorHolder : ScriptableObject
{
    [SerializeField] private Color[] _colors;

    public IReadOnlyList<Color> Colors => _colors;

    private void OnValidate()
    {
        const int ColorCount = 3;

        if (_colors.Length != ColorCount)
        {
            Array.Resize(ref _colors, ColorCount);
        }
    }
}