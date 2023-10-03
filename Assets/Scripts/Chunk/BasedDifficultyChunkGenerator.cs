using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class BasedDifficultyChunkGenerator : ChunkGenerator
{
    [Range(1, 50)][SerializeField] private int _chunksToGenerate = 5;
    [Range(0, 100)][SerializeField] private int _rotateChance = 50;
    [SerializeField] private LevelLoadConfig _levelLoadConfig;
    [SerializeField] private bool _debugLog;
    [SerializeField] protected List<Chunk> _availableChunks;

    int _level;
    const float RotationDegree = 180f;

    public void Init(int level)
    {
        _level = level;
    }

    protected override void OnStartGeneration()
    {
        if (ChunkStorage.Instance.Chunks?.Count > 0)
        {
            GenerateLevel(ChunkStorage.Instance.Chunks);
        }
        else
        {
            var chunkComposer = new ChunkComposer(_availableChunks, _chunksToGenerate);
            List<Chunk> chunks = chunkComposer.GetSuitableChunks(_level, _chunksToGenerate);

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
                string newChunkName = newChunk.name;

                if (_levelLoadConfig.IsUsedChunk(newChunkName, out rotation))
                {
                    rotation *= Quaternion.Euler(0, RotationDegree, 0);
                }
                else
                {
                    rotation = GetRandomRotation(newChunk);
                }

                var chunkData = new ChunkData(newChunkName, rotation);
                ChunkStorage.Instance.Add(chunkData);

                _levelLoadConfig.AddUsedChunk(chunkData);
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