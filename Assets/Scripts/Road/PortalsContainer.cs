using System.Collections.Generic;
using UnityEngine;

public class PortalsContainer : MonoBehaviour
{
    [SerializeField] private ChunkGenerator _chunkGenerator;

    private Portal[] _portals;

    public IReadOnlyList<Portal> Portals => _portals;

    private void OnEnable()
    {
        _chunkGenerator.Completed += OnChunkGeneratorCompleted;
    }

    private void OnDisable()
    {
        _chunkGenerator.Completed -= OnChunkGeneratorCompleted;
    }

    private void OnChunkGeneratorCompleted()
    {
        _portals = FindObjectsOfType<Portal>();
    }
}