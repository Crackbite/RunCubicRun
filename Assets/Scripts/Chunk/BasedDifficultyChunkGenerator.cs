using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class BasedDifficultyChunkGenerator : ChunkGenerator
{
    [Range(1, 50)][SerializeField] private int _chunksToGenerate = 5;
    [Range(0, 100)][SerializeField] private int _rotateChance = 50;
    [SerializeField] private bool _debugLog;
    [SerializeField] protected Chunk[] _availableChunks;

    protected override void OnStartGeneration()
    {
        if (ChunkStorage.Instance.Chunks?.Count > 0)
        {
            GenerateLevel(ChunkStorage.Instance.Chunks);
        }
        else
        {
            var chunkComposer = new ChunkComposer(_availableChunks);
            List<Chunk> chunks = chunkComposer.GetSuitableChunks(Level, _chunksToGenerate);

            GenerateLevel(chunks);
        }

        CompleteGeneration();
    }

    protected override void GenerateLevel(IReadOnlyList<object> chunks)
    {
        Chunk lastChunk = StarterChunk;
        int chunksNumber = Mathf.Min(_chunksToGenerate, chunks.Count);

        for (int i = 0; i < chunksNumber; i++)
        {
            Quaternion rotation;

            if (chunks[i] is Chunk newChunk)
            {
                rotation = GetRandomRotation(newChunk);

                var chunkData = new ChunkData(newChunk.name, rotation);
                ChunkStorage.Instance.Add(chunkData);
            }
            else if (chunks[i] is ChunkData chunkData)
            {
                newChunk = _availableChunks.FirstOrDefault(chunk => chunkData.Name.StartsWith(chunk.name));

                if (newChunk == null)
                {
                    continue;
                }

                rotation = chunkData.Rotation;
            }
            else
            {
                continue;
            }

            Vector3 chunkPosition = CalculateNewChunkPosition(lastChunk, newChunk);
            lastChunk = Instantiate(newChunk, chunkPosition, rotation, ChunkContainer);
            _chunks.Add(lastChunk);
        }

        FinalChunk.transform.position = CalculateNewChunkPosition(lastChunk, FinalChunk);
        OutputDebugInformation();
    }

    private Quaternion GetRandomRotation(Chunk chunk)
    {
        const float RotationDegree = 180f;

        Quaternion defaultRotation = Quaternion.identity;
        Quaternion newRotation = Quaternion.Euler(0f, RotationDegree, 0f);

        if (chunk.CanRotate == false)
        {
            return defaultRotation;
        }

        int chance = Random.Range(0, 100);
        return chance > _rotateChance ? defaultRotation : newRotation;
    }

    private void OutputDebugInformation()
    {
        if (_debugLog)
        {
            Debug.Log(ChunkStorage.Instance.GetChunksData());
        }
    }
}