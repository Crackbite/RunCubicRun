using System.Collections.Generic;
using UnityEngine;

public class TrainingChunkGenerator : ChunkGenerator
{
    [SerializeField] private TrainingStageHolder _trainingStageHolder;

    protected override void OnStartGeneration()
    {
        if (_trainingStageHolder.TryGetStageInfo(TraininStage, out TrainingStageInfo currentStageInfo))
        {
            GenerateLevel(currentStageInfo.Chunks);
            CompleteGeneration();
            return;
        }

        CompleteGeneration();
    }

    protected override void GenerateLevel(IReadOnlyList<object> chunks)
    {
        Chunk lastChunk = StarterChunk;

        foreach (Chunk chunk in chunks)
        {
            Vector3 chunkPosition = CalculateNewChunkPosition(lastChunk, chunk);
            lastChunk = Instantiate(chunk, chunkPosition, Quaternion.identity, ChunkContainer);
            _chunks.Add(lastChunk);
        }

        FinalChunk.transform.position = CalculateNewChunkPosition(lastChunk, FinalChunk);
    }
}
