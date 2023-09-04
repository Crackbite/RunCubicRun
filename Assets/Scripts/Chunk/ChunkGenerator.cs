using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChunkGenerator : MonoBehaviour
{
    [SerializeField] private AuthRequestScreen _authRequestScreen;
    [SerializeField] protected Chunk StarterChunk;
    [SerializeField] protected Chunk FinalChunk;
    [SerializeField] protected Transform ChunkContainer;

    protected List<Chunk> _chunks;
    protected int Level;
    protected int TraininStage;

    public event Action Completed;
    public event Action ChunksRemoved;

    public IReadOnlyList<Chunk> Chunks => _chunks;

    private void OnEnable()
    {
        _chunks = new List<Chunk>();
        OnStartGeneration();
        _authRequestScreen.PlayerAuthorized += OnPlayerAuthorized;
    }

    private void OnDisable()
    {
        _authRequestScreen.PlayerAuthorized -= OnPlayerAuthorized;
    }
    public void Init(int level, int trainingStage)
    {
        Level = level;
        TraininStage = trainingStage;
    }

    protected abstract void OnStartGeneration();
    protected abstract void GenerateLevel(IReadOnlyList<object> chunks);

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

    private void RemoveChunksFromScene()
    {
        foreach (Transform child in ChunkContainer)
        {
            Destroy(child.gameObject);
        }
    }

    private void OnPlayerAuthorized()
    {
        _chunks.Clear();
        enabled = false;
        RemoveChunksFromScene();
        ChunksRemoved?.Invoke();
    }
}
