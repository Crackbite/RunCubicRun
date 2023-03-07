using System.Collections.Generic;
using UnityEngine;

public class BlocksContainer : MonoBehaviour
{
    [SerializeField] private BlockStacker _blockStacker;

    private readonly List<ColorBlock> _blocks = new();

    private void OnEnable()
    {
        _blockStacker.ColorBlockAdded += OnColorBlockAdded;
    }

    private void Update()
    {
        transform.position = new Vector3(
            _blockStacker.transform.position.x,
            transform.position.y,
            transform.position.z);
    }

    private void OnDisable() 
    {
        _blockStacker.ColorBlockAdded -= OnColorBlockAdded;
    }

    private void OnColorBlockAdded(ColorBlock colorBlock)
    {
        _blocks.Add(colorBlock);
        colorBlock.EnableFollow(_blockStacker.transform);

        for (int i = 0; i < _blocks.Count; i++)
        {
            _blocks[i].SetHeightPosition(_blocks.Count - i);
        }
    }
}