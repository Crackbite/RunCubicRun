using System;
using UnityEngine;

public class BlockDestroyer : MonoBehaviour
{
    [SerializeField] private BlockStack _blockStack;
    [SerializeField] private int _minBlocksToLeave = 8;

    public event Action LeavePressAllowed;

    public void DestroyBlock(ColorBlock colorBlock)
    {
        if (_blockStack.Blocks.Count <= _minBlocksToLeave + 1)
        {
            LeavePressAllowed?.Invoke();
        }

        _blockStack.Destroy(colorBlock);
    }
}