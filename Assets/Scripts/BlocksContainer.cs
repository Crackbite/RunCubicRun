using System;
using System.Collections.Generic;
using UnityEngine;

public class BlocksContainer : MonoBehaviour
{
    [SerializeField] private BlockStacker _blockStacker;
    [SerializeField] private FollowController _followController;

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

    public void PlaceInStack(ColorBlock block)
    {
        block.Init(this, _followController);
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
        PlaceInStack(colorBlock);
    }
}