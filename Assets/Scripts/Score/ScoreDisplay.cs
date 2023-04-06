using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private ScoreAllocator _scoreAllocator;
    [SerializeField] private bool _isDebug;

    private TMP_Text _score;

    private string _scoreText;

    private void OnEnable()
    {
        _scoreAllocator.ScoreChanged += OnScoreChanged;
    }

    private void Start()
    {
        _score = GetComponent<TMP_Text>();
        _scoreText = _score.text;

        SetScore(0);
    }

    private void OnDisable()
    {
        _scoreAllocator.ScoreChanged -= OnScoreChanged;
    }

    private void OnScoreChanged(Score score)
    {
        SetScore(score.Current);

        if (_isDebug)
        {
            Debug.Log($"{score.Current} | {score.Change} | {score.Initiator}");
        }
    }

    private void SetScore(float score)
    {
        _score.text = $"{_scoreText}{score}";
    }
}