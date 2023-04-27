using System.Collections.Generic;
using UnityEngine;

public class ColorBlocksContainer : MonoBehaviour
{
    [SerializeField] private ChunkGenerator _chunkGenerator;

    private ColorBlock[] _colorBlocks;

    public IReadOnlyList<ColorBlock> ColorBlocks => _colorBlocks;

    private void OnEnable()
    {
        _chunkGenerator.Completed += OnChunkGeneratorCompleted;
    }

    private void OnDisable()
    {
        _chunkGenerator.Completed -= OnChunkGeneratorCompleted;
    }

    private void OnChunkGeneratorCompleted()
    {
        _colorBlocks = FindObjectsOfType<ColorBlock>();
    }
}