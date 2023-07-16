using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameTrainer : MonoBehaviour
{
    [SerializeField] private LevelEntryPortal _levelEntryPortal;
    [SerializeField] private CheckpointContainer _checkpointContainer;
    [SerializeField] private List<string> _phrases;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private float _delay;

    private Coroutine _trainRoutine;
    private WaitForSeconds _waitForSeconds;
    private bool _isCheckpointPassed;
    private int _nextPhraseNumber;

    public bool IsTrainingOver { get; private set; }

    private void OnEnable()
    {
        _levelEntryPortal.ThrownOut += OnCubicThrownOut;
    }

    private void Start()
    {
        _waitForSeconds = new WaitForSeconds(_delay);
    }

    private void OnDisable()
    {
        _levelEntryPortal.ThrowingOut -= OnCubicThrownOut;
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

        if (_phrases.Count > _nextPhraseNumber)
        {
            _isCheckpointPassed = true;
            StartCoroutine(Train(_phrases[_nextPhraseNumber]));
        }
    }

    private IEnumerator Train(string nextPhrase)
    {
        const float OpacityValue = 1.0f;
        const float TransitionDuration = 2.0f;

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
        });

        _isCheckpointPassed = false;
        _nextPhraseNumber++;
    }
}
