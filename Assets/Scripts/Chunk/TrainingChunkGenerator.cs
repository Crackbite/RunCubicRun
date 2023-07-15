using UnityEngine;

public class TrainingChunkGenerator : ChunkGenerator
{
    private void Start()
    {
        GenerateLevel(AvailableChunks[0]);
        CompleteGeneration();
    }

    private void GenerateLevel(Chunk chunk)
    {
        Chunk lastChunk = StarterChunk;
        Vector3 chunkPosition = CalculateNewChunkPosition(lastChunk, chunk);

        lastChunk = Instantiate(chunk, chunkPosition, Quaternion.identity, ChunkContainer);
        FinalChunk.transform.position = CalculateNewChunkPosition(lastChunk, FinalChunk);
    }
}
