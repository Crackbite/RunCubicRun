using System.Collections.Generic;
using UnityEngine;

public class CheckpointContainer : MonoBehaviour
{
    [SerializeField] private TrainingChunkGenerator _trainingChunkGenerator;

    private Checkpoint[] _checkpoints;

    public IReadOnlyList<Checkpoint> Checkpoints => _checkpoints;

    private void OnEnable()
    {
        _trainingChunkGenerator.Completed += OnChunkGeneratorCompleted;
    }

    private void OnDisable()
    {
        _trainingChunkGenerator.Completed -= OnChunkGeneratorCompleted;
    }

    private void OnChunkGeneratorCompleted()
    {
        _checkpoints = FindObjectsOfType<Checkpoint>();
    }
}
