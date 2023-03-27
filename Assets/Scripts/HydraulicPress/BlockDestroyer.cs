using System;
using UnityEngine;

public class BlockDestroyer : MonoBehaviour
{
    [SerializeField] private ColorBlockCollection _blockCollection;
    [SerializeField] private int _minBlocksToLeave = 8;

    public event Action LeavePressAllowed;

    public void DestroyBlock(ColorBlock colorBlock)
    {
        if (_blockCollection.Blocks.Count <= _minBlocksToLeave + 1)
        {
            LeavePressAllowed?.Invoke();
        }

        _blockCollection.Destroy(colorBlock);
    }
}