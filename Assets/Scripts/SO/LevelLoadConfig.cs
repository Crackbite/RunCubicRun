using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelLoadConfig", menuName = "LevelData/LevelLoadConfig", order = 51)]
public class LevelLoadConfig : ScriptableObject
{
    private List<ChunkData> _usedChunks = new List<ChunkData>();

    public bool IsStartWithoutMenu { get; set; }

    public IReadOnlyList<ChunkData> UsedChunks => _usedChunks;

    private void Awake()
    {
        ClearUsedChunks();
    }

    public void AddUsedChunk(ChunkData chunk)
    {
        for (int i = 0; i < _usedChunks.Count; i++)
        {
            if (_usedChunks[i].Name == chunk.Name)
            {
                _usedChunks[i] = chunk;
                return;
            }
        }

        _usedChunks.Add(chunk);
    }

    public bool IsUsedChunk(string chunkName, out Quaternion rotation)
    {
        rotation = Quaternion.identity;

        foreach (ChunkData usedChunk in _usedChunks)
        {
            if (usedChunk.Name == chunkName)
            {
                rotation = usedChunk.Rotation;
                return true;
            }
        }

        return false;
    }

    public void ClearUsedChunks()
    {
        _usedChunks.Clear();
    }
}
