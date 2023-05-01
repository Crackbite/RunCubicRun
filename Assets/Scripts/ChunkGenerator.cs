using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChunkGenerator : MonoBehaviour
{
    [SerializeField] private Chunk _startChunk;
    [SerializeField] private Chunk[] _availableChunks;
    [SerializeField] private Chunk _finishChunk;
    [SerializeField] private Transform _chunkContainer;
    [Range(1, 50)] [SerializeField] private int _chunksNumber;
    [Range(0, 100)] [SerializeField] private int _rotateChance;

    public event Action Completed;

    private void Start()
    {
        var chunkComposer = new ChunkComposer(_availableChunks);
        List<Chunk> chunks = chunkComposer.GetSuitableChunks(1, _chunksNumber);

        GenerateLevel(chunks);
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

    private void GenerateLevel(List<Chunk> chunks)
    {
        Vector3 chunkPosition;
        Chunk lastChunk = _startChunk;
        int chunksNumber = Mathf.Min(_chunksNumber, chunks.Count);

        for (int i = 0; i < chunksNumber; i++)
        {
            Chunk newChunk = chunks[i];
            chunkPosition = CalculateNewChunkPosition(lastChunk, newChunk);

            Quaternion rotation = GetRandomRotation(newChunk);
            lastChunk = Instantiate(newChunk, chunkPosition, rotation, _chunkContainer);
        }

        chunkPosition = CalculateNewChunkPosition(lastChunk, _finishChunk);
        _finishChunk.transform.position = chunkPosition;
    }

    private float GetChunkWidth(Chunk chunk)
    {
        var meshRenderer = chunk.GetComponentInChildren<Road>().GetComponent<MeshRenderer>();
        return meshRenderer.bounds.extents.x;
    }

    private Quaternion GetRandomRotation(Chunk chunk)
    {
        if (chunk.CanRotate == false)
        {
            return Quaternion.identity;
        }

        int chance = Random.Range(0, 100);
        return chance > _rotateChance ? Quaternion.identity : Quaternion.Euler(0f, 180f, 0f);
    }
}