using UnityEngine;

public class GameTrainer : MonoBehaviour
{
    [SerializeField] private LevelEntryPortal _levelEntryPortal;
    [SerializeField] private CheckpointContainer _checkpointContainer;
    [SerializeField] private TrainingPhraseDisplay _phraseDisplay;
    [SerializeField] private CubicInputHandler _cubicInputHandler;
    [SerializeField] private TrainingScreen _trainingScreen;
    [SerializeField] private Cubic _cubic;

    private int _nextPhraseNumber;
    private bool _isGamePaused;

    public TrainingScreen TrainingScreen => _trainingScreen;

    private void OnEnable()
    {
        _levelEntryPortal.ThrownOut += OnCubicThrownOut;
        _cubicInputHandler.PressSpeedReduced += OnPressSpeedReduced;
        _phraseDisplay.Completed += OnPhraseDisplayCompleted;
        _cubic.Hit += OnCubicHit;
    }

    private void OnDisable()
    {
        _levelEntryPortal.ThrownOut -= OnCubicThrownOut;
        _cubicInputHandler.PressSpeedReduced -= OnPressSpeedReduced;
        _phraseDisplay.Completed -= OnPhraseDisplayCompleted;
        _cubic.Hit -= OnCubicHit;
    }

    public void StartTraining()
    {
        if (_phraseDisplay.PhrasesAmount > _nextPhraseNumber)
        {
            _phraseDisplay.Display(_nextPhraseNumber);
            _trainingScreen.Enter();
            _nextPhraseNumber++;
        }
    }

    private void EndTraining()
    {
        if (_isGamePaused && _phraseDisplay.CanEndTrainingPause())
        {
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
}
