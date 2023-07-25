using UnityEngine;

public class TrainingChunkGenerator : ChunkGenerator
{
    [SerializeField] private GameDataHandler _gameDataHandler;

    private void Start()
    {
        GenerateLevel();
        CompleteGeneration();
    }

    private void GenerateLevel()
    {
        Chunk lastChunk = StarterChunk;

        foreach (Chunk chunk in AvailableChunks)
        {
            Vector3 chunkPosition = CalculateNewChunkPosition(lastChunk, chunk);
            lastChunk = Instantiate(chunk, chunkPosition, Quaternion.identity, ChunkContainer);
        }

        FinalChunk.transform.position = CalculateNewChunkPosition(lastChunk, FinalChunk);
    }
}
