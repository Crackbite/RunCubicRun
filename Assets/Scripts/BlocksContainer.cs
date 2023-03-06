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

        if (_blocks.Count > 1)
        {
            _blocks[_blocks.Count - 2].EnableFollow(colorBlock.transform);
        }

        colorBlock.EnableFollow(_blockStacker.transform);
    }
}