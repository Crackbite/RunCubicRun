using TMPro;
using UnityEngine;

public class DebugOutput : MonoBehaviour
{
    [SerializeField] private TMP_Text _outputText;
    [SerializeField] private BasedDifficultyChunkGenerator _chunkGenerator;

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
        _outputText.text = ChunkStorage.Instance.GetChunksData();
    }
}