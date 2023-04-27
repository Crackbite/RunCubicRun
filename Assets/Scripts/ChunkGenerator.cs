using System;
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
        ShuffleChunks(_availableChunks);
        GenerateLevel();

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

    private void GenerateLevel()
    {
        Vector3 chunkPosition;
        Chunk lastChunk = _startChunk;
        int chunksNumber = Mathf.Min(_chunksNumber, _availableChunks.Length);

        for (int i = 0; i < chunksNumber; i++)
        {
            Chunk newChunk = _availableChunks[i];
            chunkPosition = CalculateNewChunkPosition(lastChunk, newChunk);

            Quaternion rotation = GetRandomRotation();
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

    private Quaternion GetRandomRotation()
    {
        const int MaxChance = 99;

        int range = Random.Range(0, MaxChance);

        return range >= _rotateChance ? Quaternion.identity : Quaternion.Euler(0f, 180f, 0f);
    }

    private void ShuffleChunks(Chunk[] chunks)
    {
        for (int i = chunks.Length - 1; i > 0; i--)
        {
            int range = Random.Range(0, i + 1);
            (chunks[i], chunks[range]) = (chunks[range], chunks[i]);
        }
    }
}