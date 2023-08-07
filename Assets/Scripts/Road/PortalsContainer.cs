using System.Collections.Generic;
using UnityEngine;

public class PortalsContainer : MonoBehaviour
{
    [SerializeField] private BonusHandler _bonusHandler;
    [SerializeField] private LevelGenerationStarter _generatorStarter;

    private ChunkGenerator _currentGenerator;
    private Portal[] _portals;

    public IReadOnlyList<Portal> Portals => _portals;

    private void OnEnable()
    {
        _generatorStarter.GeneratorStarted += OnGeneratorStarted;
    }

    private void OnDisable()
    {
        _currentGenerator.Completed -= OnChunkGenerationCompleted;
        _generatorStarter.GeneratorStarted -= OnGeneratorStarted;
    }

    private void OnChunkGenerationCompleted()
    {
        _portals = FindObjectsOfType<Portal>();

        foreach (Portal portal in _portals)
        {
            portal.Init(_bonusHandler);
        }

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

    private void OnGeneratorStarted(ChunkGenerator currentGenerator)
    {
        _currentGenerator = currentGenerator;
        _currentGenerator.Completed += OnChunkGenerationCompleted;
    }
}