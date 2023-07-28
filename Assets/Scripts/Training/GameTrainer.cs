using UnityEngine;

public class GameTrainer : MonoBehaviour
{
    [SerializeField] private LevelEntryPortal _levelEntryPortal;
    [SerializeField] private CheckpointContainer _checkpointContainer;
    [SerializeField] private PistonPresser _pistonPresser;
    [SerializeField] private CubicInputHandler _cubicInputHandler;
    [SerializeField] private CubicMovement _cubicMovement;
    [SerializeField] private TrainingPhraseDisplay _phraseDisplay;
    [SerializeField] private bool _hasPressTraining;

    private int _nextPhraseNumber;
    private bool _canStopGame;

    public bool IsTrainingOver { get; private set; }

    private void OnEnable()
    {
        _levelEntryPortal.ThrownOut += OnCubicThrownOut;
        _cubicMovement.CubicLeftPress += OnCubicLeftPress;
        _pistonPresser.CubicReached += OnPressCubicReached;

        if (_hasPressTraining)
        {
            _phraseDisplay.DisplayCompleted += OnDisplayCompleted;
            _pistonPresser.StackReached += OnStackReached;
            _cubicInputHandler.PressSpeedReduced += OnPressSpeedReduced;
        }
    }

    private void OnDisable()
    {
        _levelEntryPortal.ThrownOut -= OnCubicThrownOut;
        _cubicMovement.CubicLeftPress -= OnCubicLeftPress;
        _pistonPresser.CubicReached -= OnPressCubicReached;

        if (_hasPressTraining)
        {
            _phraseDisplay.DisplayCompleted -= OnDisplayCompleted;
            _pistonPresser.StackReached -= OnStackReached;
            _cubicInputHandler.PressSpeedReduced -= OnPressSpeedReduced;
        }
    }

    private void StartTraining()
    {
        const float Delay = 0.5f;

        if (_phraseDisplay.PhrasesAmount > _nextPhraseNumber)
        {
            _phraseDisplay.CleanText();
            Invoke(nameof(Train), Delay);
        }
    }

    private void Train()
    {
        _phraseDisplay.Display(_nextPhraseNumber);
        _nextPhraseNumber++;
    }

    private void OnDisplayCompleted()
    {
        if (_canStopGame)
        {
            Time.timeScale = 0f;
            _canStopGame = false;
        }
    }

    private void OnPressCubicReached(Cubic cubic)
    {
        _phraseDisplay.CleanText();
    }

    private void OnCubicLeftPress()
    {
        StartTraining();
    }

    private void OnPressSpeedReduced()
    {
        _cubicInputHandler.PressSpeedReduced -= OnPressSpeedReduced;
        Time.timeScale = 1f;
        StartTraining();
    }

    private void OnStackReached()
    {
        _canStopGame = true;
        StartTraining();
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
