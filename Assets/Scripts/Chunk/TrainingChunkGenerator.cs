using System.Collections.Generic;
using UnityEngine;

public class TrainingChunkGenerator : ChunkGenerator
{
    [SerializeField] private TrainingStageHolder _trainingStageHolder;

    private void OnEnable()
    {
        int stage = DataRestorer.TrainingStageNumber;

            if (_trainingStageHolder.TryGetStageInfo(stage, out TrainingStageInfo currentStageInfo))
            {
                GenerateLevel(currentStageInfo.Chunks);
                CompleteGeneration();
                return;
            }

        CompleteGeneration();
    }

    private void GenerateLevel(IReadOnlyList<Chunk> availableChunks)
    {
        Chunk lastChunk = StarterChunk;

        foreach (Chunk chunk in availableChunks)
        {
            Vector3 chunkPosition = CalculateNewChunkPosition(lastChunk, chunk);
            lastChunk = Instantiate(chunk, chunkPosition, Quaternion.identity, ChunkContainer);
        }

        FinalChunk.transform.position = CalculateNewChunkPosition(lastChunk, FinalChunk);
    }
}
