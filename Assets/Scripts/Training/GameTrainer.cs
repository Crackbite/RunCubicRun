using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTrainer : MonoBehaviour
{
    [SerializeField] private LevelEntryPortal _levelEntryPortal;
    [SerializeField] private CheckpointContainer _checkpointContainer;
    [SerializeField] private PistonPresser _pistonPresser;
    [SerializeField] private CubicInputHandler _cubicInputHandler;
    [SerializeField] private CubicMovement _cubicMovement;
    [SerializeField] private TrainingPhraseDisplay _phraseDisplay;
    [SerializeField] private List<string> _phrases;
    [SerializeField] private float _delay;
    [SerializeField] private bool _hasPressTraining;

    private Coroutine _trainRoutine;
    private WaitForSeconds _waitForSeconds;
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

    private void Start()
    {
        _waitForSeconds = new WaitForSeconds(_delay);
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
        _trainRoutine = StartCoroutine(Train());

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

    private void StartTraining()
    {
        if (_phraseDisplay.PhrasesAmount > _nextPhraseNumber)
        {
            StartCoroutine(Train());
        }
    }

    private IEnumerator Train()
    {
        _phraseDisplay.CleanText();
        yield return _waitForSeconds;
        _phraseDisplay.Display(_nextPhraseNumber);
        _nextPhraseNumber++;
    }
}
