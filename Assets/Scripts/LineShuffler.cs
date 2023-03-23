using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LineShuffler : MonoBehaviour
{
    [SerializeField] private List<GameObject> _lines;
    [SerializeField] private ColorHolder _colorHolder;

    private readonly List<Color> _colors = new();

    private void Start()
    {
        foreach (Color color in _colorHolder.Colors)
        {
            _colors.Add(color);
        }

        Shuffle();
    }

    public void Shuffle()
    {
        float maxValue = _colorHolder.Colors.Count - 1;

        for (int i = _colors.Count - 1; i > 0; i--)
        {
            int randomIndex = Convert.ToInt32(Mathf.Round(Random.Range(0, maxValue)));
            (_colors[i], _colors[randomIndex]) = (_colors[randomIndex], _colors[i]);
        }

        AssignColorsToLines();
    }

    private void AssignColorsToLines()
    {
        for (int i = 0; i < _lines.Count; i++)
        {
            ColorBlockRenderer[] blocks = _lines[i].GetComponentsInChildren<ColorBlockRenderer>();

            foreach (ColorBlockRenderer block in blocks)
            {
                block.SetColor(_colors[i]);
            }
        }
    }
}