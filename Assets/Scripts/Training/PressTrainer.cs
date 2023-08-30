using System.Collections;
using UnityEngine;

public class PressTrainer : MonoBehaviour
{
    [SerializeField] private Cubic _cubic;
    [SerializeField] private GameTrainer _gameTrainer;
    [SerializeField] private DataRestorer _dataRestorer;

    private void OnEnable()
    {
        _dataRestorer.DataRestored += OnDataRestored;
        _cubic.SteppedOnStand += OnCubicSteppedOnStand;
    }

    private void OnDisable()
    {
        _dataRestorer.DataRestored -= OnDataRestored;
        _cubic.SteppedOnStand -= OnCubicSteppedOnStand;
    }

    private void OnCubicSteppedOnStand(PressStand stand)
    {
        const float delay = 1.7f;
        _gameTrainer.TrainingScreen.ChangeWindowAnimation();
        StartCoroutine(StartTraining(delay));
    }

    private void OnDataRestored()
    {
        int workStageNumber = 1;

        if (_dataRestorer.TrainingStageNumber != workStageNumber)
        {
            enabled = false;
        }
    }

    private IEnumerator StartTraining(float delay)
    {
        yield return new WaitForSeconds(delay);
        _gameTrainer.StartTraining();
    }
}
