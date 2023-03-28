using System;
using System.Collections.Generic;
using UnityEngine;

public class BlockStack : MonoBehaviour
{
    [SerializeField] private Cubic _cubic;
    [SerializeField] private BlockStackCoordinator _stackCoordinator;

    private readonly List<ColorBlock> _blocks = new();

    public event Action<ColorBlock> BlockAdded;
    public event Action<ColorBlock> BlockRemoved;

    public IReadOnlyList<ColorBlock> Blocks => _blocks;

    private void Update()
    {
        Vector3 currentPosition = transform.position;
        transform.position = new Vector3(_cubic.transform.position.x, currentPosition.y, currentPosition.z);
    }

    public void Add(ColorBlock colorBlock)
    {
        _blocks.Add(colorBlock);
        PlaceInStack(colorBlock);

        BlockAdded?.Invoke(colorBlock);
    }

    public void Destroy(ColorBlock colorBlock, float delay = 0f)
    {
        _blocks.Remove(colorBlock);
        BlockRemoved?.Invoke(colorBlock);

        Destroy(colorBlock.gameObject, delay);
    }

    public int GetStackPosition(ColorBlock targetBlock)
    {
        int position = 0;

        for (int i = 0; i < _blocks.Count; i++)
        {
            if (_blocks[i] == targetBlock)
            {
                position = _blocks.Count - i;
                return position;
            }
        }

        return position;
    }

    public void PlaceInStack(ColorBlock colorBlock)
    {
        colorBlock.Init(this, _stackCoordinator);
    }
}