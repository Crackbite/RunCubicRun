using System;
using System.Collections;
using UnityEngine;

public class GameTrainer : MonoBehaviour
{
    [SerializeField] private LevelEntryPortal _levelEntryPortal;
    [SerializeField] private CheckpointContainer _checkpointContainer;
    [SerializeField] private TrainingPhraseDisplay _phraseDisplay;
    [SerializeField] private CubicInputHandler _cubicInputHandler;
    [SerializeField] private TrainingScreen _trainingScreen;
    [SerializeField] private Cubic _cubic;
    [SerializeField] private SettingsScreen _settingsScreen;

    private int _nextPhraseNumber;
    private bool _isGamePaused;
    private bool _isSettingsShowed;

    public event Action TrainingStarted;
    public event Action TrainingEnded;

    public TrainingScreen TrainingScreen => _trainingScreen;

    private void OnEnable()
    {
        _levelEntryPortal.ThrownOut += OnCubicThrownOut;
        _cubicInputHandler.PressSpeedReduced += OnPressSpeedReduced;
        _phraseDisplay.Completed += OnPhraseDisplayCompleted;
        _cubic.Hit += OnCubicHit;
        _settingsScreen.Showing += OnSettingsScreenShowing;
        _settingsScreen.Hidden += OnSettingsScreenHidden;
    }

    private void OnDisable()
    {
        _levelEntryPortal.ThrownOut -= OnCubicThrownOut;
        _cubicInputHandler.PressSpeedReduced -= OnPressSpeedReduced;
        _phraseDisplay.Completed -= OnPhraseDisplayCompleted;
        _cubic.Hit -= OnCubicHit;
        _settingsScreen.Showing -= OnSettingsScreenShowing;
        _settingsScreen.Hidden -= OnSettingsScreenHidden;
    }

    public void StartTraining()
    {
        if (_phraseDisplay.PhrasesAmount > _nextPhraseNumber)
        {
            if (_isSettingsShowed)
            {
                StartCoroutine(WaitForSettingsHidden(() => StartTraining()));
                return;
            }

            TrainingStarted.Invoke();
            _phraseDisplay.Display(_nextPhraseNumber);
            _trainingScreen.Enter();
            _nextPhraseNumber++;
        }
    }

    private void EndTraining()
    {
        if (_isGamePaused && _phraseDisplay.CanEndTrainingPause())
        {
            if (_isSettingsShowed)
            {
                StartCoroutine(WaitForSettingsHidden(() => EndTraining()));
                return;
            }

            TrainingEnded?.Invoke();
            _trainingScreen.Exit();
            _phraseDisplay.End();
        }
    }

    private void OnPressSpeedReduced()
    {
        EndTraining();
    }

    private void OnPhraseDisplayCompleted()
    {
        const int PauseValue = 0;

        _isGamePaused = Time.timeScale == PauseValue;
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

    private void OnCubicHit(Vector3 _, float __)
    {
        _trainingScreen.Exit();
        _phraseDisplay.EndImmediately();
    }

    private void OnSettingsScreenShowing()
    {
        _isSettingsShowed = true;
    }

    private void OnSettingsScreenHidden()
    {
        _isSettingsShowed = false;
    }

    private IEnumerator WaitForSettingsHidden(Action callBack)
    {
        while (_isSettingsShowed)
        {
            yield return null;
        }

        callBack();
    }
}
