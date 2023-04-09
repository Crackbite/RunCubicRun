﻿using System.Collections.Generic;
using UnityEngine;

public class BonusHandler : MonoBehaviour
{
    [SerializeField] private Cubic _cubic;
    [SerializeField] private BonusIcon _bonusIconPrefab;
    [SerializeField] private Transform _bonusIconsPanel;

    private Dictionary<BonusInfo, BonusItem> _activeBonuses;

    private void Awake()
    {
        _activeBonuses = new Dictionary<BonusInfo, BonusItem>();
    }

    private void OnEnable()
    {
        _cubic.BonusReceived += OnCubicBonusReceived;
    }

    private void OnDisable()
    {
        _cubic.BonusReceived -= OnCubicBonusReceived;

        foreach ((BonusInfo _, BonusItem bonusItem) in _activeBonuses)
        {
            UnsubscribeFromBonusTimerEvents(bonusItem.BonusTimer);
        }
    }

    private void ActivateBonus(Bonus bonus)
    {
        var bonusTimer = new BonusTimer(bonus);
        bonusTimer.TimerChanged += OnTimerChanged;
        bonusTimer.TimerFinished += OnTimerFinished;

        BonusIcon bonusIcon = Instantiate(_bonusIconPrefab, _bonusIconsPanel);
        bonusIcon.Init(bonus.Info.Icon, bonusTimer.RemainingSeconds);

        var bonusItem = new BonusItem(bonus, bonusIcon, bonusTimer);
        _activeBonuses.Add(bonus.Info, bonusItem);

        bonus.Apply();
        bonusTimer.Start();
    }

    private void DeactivateBonus(BonusItem bonusItem)
    {
        bonusItem.Bonus.Cancel();
        Destroy(bonusItem.BonusIcon.gameObject);

        UnsubscribeFromBonusTimerEvents(bonusItem.BonusTimer);
        _activeBonuses.Remove(bonusItem.Bonus.Info);
    }

    private void OnCubicBonusReceived(Bonus receivedBonus)
    {
        if (_activeBonuses.TryGetValue(receivedBonus.Info, out BonusItem bonusItem))
        {
            float newBonusTime = bonusItem.BonusTimer.RemainingSeconds + receivedBonus.Info.Duration;
            bonusItem.BonusTimer.SetTime(newBonusTime);
        }
        else
        {
            ActivateBonus(receivedBonus);
        }
    }

    private void OnTimerChanged(Bonus bonus, float remainingSeconds, TimeChangingSource changingSource)
    {
        if (_activeBonuses.TryGetValue(bonus.Info, out BonusItem bonusItem))
        {
            bonusItem.BonusIcon.SetTime(remainingSeconds);
        }
    }

    private void OnTimerFinished(Bonus bonus)
    {
        if (_activeBonuses.TryGetValue(bonus.Info, out BonusItem bonusItem))
        {
            DeactivateBonus(bonusItem);
        }
    }

    private void UnsubscribeFromBonusTimerEvents(BonusTimer bonusTimer)
    {
        bonusTimer.TimerChanged -= OnTimerChanged;
        bonusTimer.TimerFinished -= OnTimerFinished;
    }
}