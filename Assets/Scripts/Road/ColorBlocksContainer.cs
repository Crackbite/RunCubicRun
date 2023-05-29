using System.Collections.Generic;
using UnityEngine;

public class ColorBlocksContainer : MonoBehaviour
{
    [SerializeField] private ChunkGenerator _chunkGenerator;
    [SerializeField] private Cubic _cubic;

    private ColorBlock[] _colorBlocks;

    public IReadOnlyList<ColorBlock> ColorBlocks => _colorBlocks;

    private void OnEnable()
    {
        _chunkGenerator.Completed += OnChunkGeneratorCompleted;
        _cubic.Hit += OnCubicHit;
    }

    private void OnDisable()
    {
        _chunkGenerator.Completed -= OnChunkGeneratorCompleted;
        _cubic.Hit -= OnCubicHit;
    }

    private void OnCubicHit(Vector3 contactPoint, float trapHeight)
    {
        foreach (ColorBlock block in _colorBlocks)
        {
            if(block != null && block.IsInStack == false)
            {
                block.BlockPhysics.TurnOffTrigger();
            }
        }
    }

    private void OnChunkGeneratorCompleted()
    {
        _colorBlocks = FindObjectsOfType<ColorBlock>();
    }
}