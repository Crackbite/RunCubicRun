using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameTrainer : MonoBehaviour
{
    [SerializeField] private LevelEntryPortal _levelEntryPortal;
    [SerializeField] private CheckpointContainer _checkpointContainer;
    [SerializeField] private PistonPresser _pistonPresser;
    [SerializeField] private CubicInputHandler _cubicInputHandler;
    [SerializeField] private CubicMovement _cubicMovement;
    [SerializeField] private int _trainingStageAmount;

    [SerializeField] private List<string> _phrases;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private float _delay;
    [SerializeField] private bool _hasPressTraining;

    private Coroutine _trainRoutine;
    private WaitForSeconds _waitForSeconds;
    private bool _isCheckpointPassed;
    private int _nextPhraseNumber;

    public bool IsTrainingOver { get; private set; }

    private void OnEnable()
    {
        _levelEntryPortal.ThrownOut += OnCubicThrownOut;
        _cubicMovement.CubicLeftPress += OnCubicLeftPress;
        _pistonPresser.CubicReached += OnPressCubicReached;

        if (_hasPressTraining)
        {
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
        _levelEntryPortal.ThrowingOut -= OnCubicThrownOut;
        _cubicMovement.CubicLeftPress -= OnCubicLeftPress;
        _pistonPresser.CubicReached -= OnPressCubicReached;

        if (_hasPressTraining)
        {
            _pistonPresser.StackReached -= OnStackReached;
            _cubicInputHandler.PressSpeedReduced -= OnPressSpeedReduced;
        }
    }

    private void OnPressCubicReached(Cubic cubic)
    {
        _text.text = null;
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
        if (_hasPressTraining)
        {
            StartTraining(_hasPressTraining);
        }
    }

    private void OnCubicThrownOut()
    {
        _trainRoutine = StartCoroutine(Train(_phrases[0]));

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

    private void StartTraining(bool isPressTraining = false)
    {
        if (_phrases.Count > _nextPhraseNumber)
        {
            _isCheckpointPassed = true;
            StartCoroutine(Train(_phrases[_nextPhraseNumber], isPressTraining));
        }
    }

    private void LoadNextStage()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    private IEnumerator Train(string nextPhrase, bool isPressTraining = false)
    {
        const float OpacityValue = 1.0f;
        const float TransitionDuration = 1.0f;

        _text.text = null;
        yield return _waitForSeconds;

        _text.text = nextPhrase;
        _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, 0f);
        Tweener tweener = null;
        tweener = _text.DOFade(OpacityValue, TransitionDuration).OnUpdate(() =>
        {
            if (_isCheckpointPassed)
            {
                tweener.Kill();
            }
        }).OnComplete(() =>
        {
            if (isPressTraining)
            {
                Time.timeScale = 0f;
            }
        });

        _isCheckpointPassed = false;
        _nextPhraseNumber++;
    }
}
