using System;
using System.Collections.Generic;
using UnityEngine;

public class BlocksContainer : MonoBehaviour
{
    [SerializeField] private BlockStacker _blockStacker;
    [SerializeField] private Cubic _cubic;

    private readonly List<ColorBlock> _blocks = new();

    private void OnEnable()
    {
        _blockStacker.ColorBlockAdded += OnColorBlockAdded;
        _cubic.Hit += OnHit;
    }

    private void Update()
    {
        Vector3 currentPosition = transform.position;
        transform.position = new Vector3(_blockStacker.transform.position.x, currentPosition.y, currentPosition.z);
    }

    private void OnDisable()
    {
        _blockStacker.ColorBlockAdded -= OnColorBlockAdded;
        _cubic.Hit -= OnHit;

    }

    private void OnHit()
    {
        Collapse();
    }

    private void OnColorBlockAdded(ColorBlock colorBlock)
    {
        _blocks.Add(colorBlock);
        colorBlock.EnableFollow(_blockStacker.transform);

        for (int i = 0; i < _blocks.Count; i++)
        {
            _blocks[i].StackPosition = _blocks.Count - i;
        }
    }

    private void Collapse()
    {
        for (int i = 0; i < _blocks.Count; i++)
        {
            _blocks[i].FallOff();
        }
    }
}