using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private ScoreAllocator _scoreAllocator;

    private void OnEnable()
    {
        _scoreAllocator.ScoreChanged += OnScoreChanged;
    }

    private void OnDisable()
    {
        _scoreAllocator.ScoreChanged -= OnScoreChanged;
    }

    private void OnScoreChanged(Score score)
    {
        Debug.Log($"{score.Current} | {score.Change}");
    }
}