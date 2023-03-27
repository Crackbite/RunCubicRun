using System;
using System.Collections.Generic;
using UnityEngine;

public class ColorBlockCollection : MonoBehaviour
{
    private readonly List<ColorBlock> _blocks = new();

    public event Action<ColorBlock> BlockAdded;
    public event Action<ColorBlock> BlockRemoved;

    public IReadOnlyList<ColorBlock> Blocks => _blocks;

    public void Add(ColorBlock colorBlock)
    {
        _blocks.Add(colorBlock);

        BlockAdded?.Invoke(colorBlock);
    }

    public void Destroy(ColorBlock colorBlock, float delay = 0f)
    {
        _blocks.Remove(colorBlock);
        BlockRemoved?.Invoke(colorBlock);

        Destroy(colorBlock.gameObject, delay);
    }
}