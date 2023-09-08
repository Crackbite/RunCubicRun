using System;
using UnityEngine;

public class LevelGenerationStarter : MonoBehaviour
{
    [SerializeField] private BasedDifficultyChunkGenerator _mainGenerator;
    [SerializeField] private TrainingChunkGenerator _trainingChunkGenerator;
    [SerializeField] private ChunkStorage _chunkStorage;
    [SerializeField] private GameObject _gameTrainer;

    public event Action<ChunkGenerator> GeneratorStarted;

    private void OnEnable()
    {
        _chunkStorage.Initialized += OnChunkStorageInitialized;
    }

    private void OnDisable()
    {
        _chunkStorage.Initialized -= OnChunkStorageInitialized;
    }

    private void StartGeneration(PlayerData playerData)
    {
        const int TrainingIndex = 0;

        if (playerData.Level > TrainingIndex)
        {
            GeneratorStarted?.Invoke(_mainGenerator);
            _mainGenerator.Init(playerData.Level);
            _mainGenerator.enabled = true;
            _gameTrainer.SetActive(false);
        }
        else
        {
            GeneratorStarted?.Invoke(_trainingChunkGenerator);
            _trainingChunkGenerator.Init(playerData.TrainingStage);
            _trainingChunkGenerator.enabled = true;
        }
    }

    private void OnChunkStorageInitialized(PlayerData playerData)
    {
        StartGeneration(playerData);
    }
}
