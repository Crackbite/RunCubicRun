using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ColorHolder", menuName = "LevelData/ColorHolder", order = 51)]
public class ColorHolder : ScriptableObject
{
    [SerializeField] List<Color> _colors;

    public IReadOnlyList<Color> Colors => _colors;
}
