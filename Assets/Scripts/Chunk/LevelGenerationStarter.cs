using System;
using UnityEngine;

public class LevelGenerationStarter : MonoBehaviour
{
    [SerializeField] private BasedDifficultyChunkGenerator _mainGenerator;
    [SerializeField] private TrainingChunkGenerator _trainingChunkGenerator;
    [SerializeField] private DataRestorer _dataRestorer;
    [SerializeField] private GameObject _gameTrainer;
 
    public event Action<ChunkGenerator> GeneratorStarted;

    private void OnEnable()
    {
        _dataRestorer.DataRestored += OnDataRestored;
    }

    private void OnDisable()
    {
        _dataRestorer.DataRestored -= OnDataRestored;
    }

    private void OnDataRestored()
    {
        const int TrainingIndex = 0;

        if (_dataRestorer.Level > TrainingIndex)
        {
           GeneratorStarted?.Invoke(_mainGenerator);
            _mainGenerator.enabled = true;
            _gameTrainer.SetActive(false);
        }
        else
        {
            GeneratorStarted?.Invoke(_trainingChunkGenerator);
            _trainingChunkGenerator.enabled = true;
        }
    }
}
