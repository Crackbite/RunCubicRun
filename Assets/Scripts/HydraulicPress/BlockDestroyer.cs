using System;
using UnityEngine;

public class BlockDestroyer : MonoBehaviour
{
    [SerializeField] private BlocksContainer _blocksContainer;
    [SerializeField] private int _minBlocksToLeave = 8;

    public event Action LeavePressAllowed;

    public void DestroyBlock(ColorBlock colorBlock)
    {
        if (_blocksContainer.BlocksCount <= _minBlocksToLeave + 1)
        {
            LeavePressAllowed?.Invoke();
        }

        _blocksContainer.DestroyBlock(colorBlock);
    }
}