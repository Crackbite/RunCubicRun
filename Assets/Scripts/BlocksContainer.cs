using System;
using System.Collections.Generic;
using UnityEngine;

public class BlocksContainer : MonoBehaviour
{
    [SerializeField] private BlockStacker _blockStacker;

    private readonly List<ColorBlock> _blocks = new();

    public event Action BlockRemoved;

    public int BlocksCount => _blocks.Count;

    private void OnEnable()
    {
        _blockStacker.ColorBlockAdded += OnColorBlockAdded;
    }

    private void Update()
    {
        Vector3 currentPosition = transform.position;
        transform.position = new Vector3(_blockStacker.transform.position.x, currentPosition.y, currentPosition.z);
    }

    private void OnDisable()
    {
        _blockStacker.ColorBlockAdded -= OnColorBlockAdded;
    }

    public void DestroyBlock(ColorBlock colorBlock)
    {
        Destroy(colorBlock.gameObject);
        _blocks.Remove(colorBlock);

        BlockRemoved?.Invoke();
    }

    public ColorBlock GetBlockByIndex(int index) => _blocks[index];

    private void OnColorBlockAdded(ColorBlock colorBlock)
    {
        _blocks.Add(colorBlock);
        colorBlock.EnableFollow(_blockStacker.transform);

        for (int i = 0; i < _blocks.Count; i++)
        {
            _blocks[i].StackPosition = _blocks.Count - i;
        }
    }
}