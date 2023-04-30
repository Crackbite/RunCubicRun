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
        SortPortalsByX();
    }

    private void SortPortalsByX()
    {
        for (int i = 1; i < _portals.Length; i++)
        {
            float keyX = _portals[i].transform.position.x;
            Portal keyPortal = _portals[i];
            int j = i - 1;

            while (j >= 0 && _portals[j].transform.position.x > keyX)
            {
                _portals[j + 1] = _portals[j];
                j--;
            }

            _portals[j + 1] = keyPortal;
        }
    }
}