using System;
using UnityEngine;

public class FirstStageTrainer : GameTrainer
{
    [SerializeField] private Cubic _cubic;

    protected override void OnEnable()
    {
        base.OnEnable();
        _cubic.SteppedOnStand += OnCubicSteppedOnStand;
    }


    protected override void OnDisable()
    {
        base.OnDisable();
        _cubic.SteppedOnStand -= OnCubicSteppedOnStand;
    }

    private void OnCubicSteppedOnStand(PressStand stand)
    {
        const float delay = 1f;

        Invoke(nameof(StartTraining), delay);
    }

    private void OnStackReached()
    {
        StartTraining();
    }
}
