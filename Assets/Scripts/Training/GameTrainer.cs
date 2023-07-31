using System;
using UnityEngine;

public class GameTrainer : MonoBehaviour
{
    [SerializeField] private LevelEntryPortal _levelEntryPortal;
    [SerializeField] private CheckpointContainer _checkpointContainer;
    [SerializeField] private TrainingPhraseDisplay _phraseDisplay;
    [SerializeField] private CubicInputHandler _cubicInputHandler;

    private int _nextPhraseNumber;
    private bool _isGamePaused;

    protected virtual void OnEnable()
    {
        _levelEntryPortal.ThrownOut += OnCubicThrownOut;
        _cubicInputHandler.PressSpeedReduced += OnPressSpeedReduced;
        _cubicInputHandler.LineChanged += OnLineChanged;
        _phraseDisplay.DisplayCompleted += OnDisplayCompleted;
    }

    protected virtual void OnDisable()
    {
        _levelEntryPortal.ThrownOut -= OnCubicThrownOut;
        _cubicInputHandler.PressSpeedReduced -= OnPressSpeedReduced;
        _cubicInputHandler.LineChanged -= OnLineChanged;
        _phraseDisplay.DisplayCompleted -= OnDisplayCompleted;
    }

    protected void StartTraining()
    {
        const float Delay = 0.5f;

        if (_phraseDisplay.PhrasesAmount > _nextPhraseNumber)
        {
            Invoke(nameof(Train), Delay);
        }
    }
    protected void Unpause()
    {
        if (_isGamePaused)
        {
            Time.timeScale = 1f;
            _phraseDisplay.CleanText();
            _isGamePaused = false;
        }
    }

    protected virtual void OnPressSpeedReduced()
    {
        Unpause();
    }

    protected virtual void OnLineChanged(Vector3 direction)
    {
        Unpause();
    }


    private void Train()
    {
        _phraseDisplay.Display(_nextPhraseNumber);
        _nextPhraseNumber++;
    }

    private void OnDisplayCompleted()
    {
        Time.timeScale = 0f;
        _isGamePaused = true;
    }


    private void OnCubicThrownOut()
    {
        StartTraining();

        foreach (Checkpoint checkPoint in _checkpointContainer.Checkpoints)
        {
            checkPoint.CubicPassed += OnCubicPassedCheckpoint;
        }
    }

    private void OnCubicPassedCheckpoint(Checkpoint passedCheckpoint)
    {
        passedCheckpoint.CubicPassed -= OnCubicPassedCheckpoint;
        StartTraining();
    }
}
