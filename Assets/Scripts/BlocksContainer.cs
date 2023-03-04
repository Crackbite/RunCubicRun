using System.Collections.Generic;
using UnityEngine;

public class BlocksContainer : MonoBehaviour
{
    [SerializeField] private BlockStacker _blockStacker;

    private List<Transform> _blocks = new List<Transform>();

    private void OnEnable()
    {
        _blockStacker.BlockAdded += OnBlockAdded;
    }

    private void OnDisable()
    {
        _blockStacker.BlockAdded -= OnBlockAdded;
    }

    private void OnBlockAdded(Transform block)
    {
        _blocks.Add(block);
    }
}