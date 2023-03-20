using System.Collections.Generic;
using UnityEngine;

public class LineShuffler : MonoBehaviour
{
    [SerializeField] private List<GameObject> _lines;
    [SerializeField] private ColorHolder _colorHolder;

    private List<Color> _colors = new List<Color>(); 

    private void Start()
    {
        foreach (Color color in _colorHolder.Colors)
        {
            _colors.Add(color);
        }

        AssignColorsToLines();
    }
    public void AssignColorsToLines()
    {
        Shuffle();
        
        for (int i = 0; i < _lines.Count; i++)
        {
            ColorBlock[] blocks = _lines[i].GetComponentsInChildren<ColorBlock>();

            foreach (ColorBlock block in blocks) 
            {
                block.SetLineColor(_colors[i]);
            }
        }
    }

    private void Shuffle()
    {
        float minValue = 0f;
        float maxValue = _colorHolder.Colors.Count - 1;

        for (int i = _colors.Count - 1; i > 0; i--)
        {
            int randomIndex = (int)Random.Range(minValue, maxValue);
            Color temporaryColor = _colors[i];
            _colors[i] = _colors[randomIndex];
            _colors[randomIndex] = temporaryColor;
        }
    }
}
