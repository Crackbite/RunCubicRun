using System.Collections.Generic;
using UnityEngine;

public class ColorBlocksContainer : MonoBehaviour
{
    [SerializeField] private Cubic _cubic;
    [SerializeField] private LevelGenerationStarter _generatorStarter;

    private ChunkGenerator _currentGenerator;
    private ColorBlock[] _colorBlocks;

    public IReadOnlyList<ColorBlock> ColorBlocks => _colorBlocks;

    private void OnEnable()
    {
        _generatorStarter.GeneratorStarted += OnGeneratorStarted;
        _cubic.Hit += OnCubicHit;
    }

    private void OnDisable()
    {
        _generatorStarter.GeneratorStarted -= OnGeneratorStarted;
        _cubic.Hit -= OnCubicHit;
    }

    private void OnCubicHit(Vector3 contactPoint, float trapHeight)
    {
        foreach (ColorBlock block in _colorBlocks)
        {
            if (block != null && block.IsInStack == false)
            {
                block.BlockPhysics.TurnOffTrigger();
            }
        }
    }

    private void OnGeneratorStarted(ChunkGenerator currentGenerator)
    {
        _currentGenerator = currentGenerator;
        _currentGenerator.Completed += OnChunkGeneratorCompleted;
    }

    private void OnChunkGeneratorCompleted()
    {
        _colorBlocks = FindObjectsOfType<ColorBlock>();
        _currentGenerator.Completed -= OnChunkGeneratorCompleted;
    }
}