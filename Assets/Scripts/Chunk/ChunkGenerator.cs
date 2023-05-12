using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChunkGenerator : MonoBehaviour
{
    [SerializeField] private Chunk _starterChunk;
    [SerializeField] private Chunk[] _availableChunks;
    [SerializeField] private Chunk _finalChunk;
    [SerializeField] private Transform _chunkContainer;
    [Range(1, 50)] [SerializeField] private int _chunksToGenerate = 5;
    [Range(0, 100)] [SerializeField] private int _rotateChance = 50;
    [SerializeField] private bool _debugLog;

    public event Action Completed;

    private void Start()
    {
        if (ChunkStorage.Instance.Chunks?.Count > 0)
        {
            GenerateLevel(ChunkStorage.Instance.Chunks);
        }
        else
        {
            var chunkComposer = new ChunkComposer(_availableChunks);
            List<Chunk> chunks = chunkComposer.GetSuitableChunks(18, _chunksToGenerate);

            GenerateLevel(chunks);
        }

        Completed?.Invoke();
    }

    private Vector3 CalculateNewChunkPosition(Chunk lastChunk, Chunk newChunk)
    {
        float lastChunkWidth = GetChunkWidth(lastChunk);
        float newChunkWidth = GetChunkWidth(newChunk);

        Vector3 chunkPosition = lastChunk.transform.position;
        chunkPosition.x += lastChunkWidth + newChunkWidth;

        return chunkPosition;
    }

    private void GenerateLevel(IReadOnlyList<object> chunks)
    {
        Chunk lastChunk = _starterChunk;
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
            lastChunk = Instantiate(newChunk, chunkPosition, rotation, _chunkContainer);
        }

        _finalChunk.transform.position = CalculateNewChunkPosition(lastChunk, _finalChunk);
        OutputDebugInformation();
    }

    private float GetChunkWidth(Chunk chunk)
    {
        var meshRenderer = chunk.GetComponentInChildren<Road>().GetComponent<MeshRenderer>();
        return meshRenderer.bounds.extents.x;
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