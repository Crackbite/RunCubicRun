using System;
using UnityEngine;

public class BlockDestroyer : MonoBehaviour
{
    [SerializeField] private BlockStack _blockStack;
    [SerializeField] private int _minBlocksToLeave = 8;
    [SerializeField] private ParticleSystem _destructionEffect;

    public event Action LeavePressAllowed;

    public void DestroyBlock(ColorBlock colorBlock)
    {
        if (_blockStack.Blocks.Count <= _minBlocksToLeave + 1)
        {
            LeavePressAllowed?.Invoke();
        }

        _blockStack.Destroy(colorBlock);
        _destructionEffect.transform.position = colorBlock.transform.position;
        _destructionEffect.Play();
    }
}