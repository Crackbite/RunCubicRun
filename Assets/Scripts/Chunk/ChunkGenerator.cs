using System;
using UnityEngine;

public abstract class ChunkGenerator : MonoBehaviour
{
    [SerializeField] protected Chunk StarterChunk;
    [SerializeField] protected Chunk FinalChunk;
    [SerializeField] protected Transform ChunkContainer;
    [SerializeField] protected DataRestorer DataRestorer;

    public event Action Completed;

    protected void CompleteGeneration()
    {
        Completed?.Invoke();
    }

    protected Vector3 CalculateNewChunkPosition(Chunk lastChunk, Chunk newChunk)
    {
        float lastChunkWidth = GetChunkWidth(lastChunk);
        float newChunkWidth = GetChunkWidth(newChunk);

        Vector3 chunkPosition = lastChunk.transform.position;
        chunkPosition.x += lastChunkWidth + newChunkWidth;

        return chunkPosition;
    }

    protected float GetChunkWidth(Chunk chunk)
    {
        var meshRenderer = chunk.GetComponentInChildren<Road>().GetComponent<MeshRenderer>();
        return meshRenderer.bounds.extents.x;
    }
}
