using System;

public class BonusTimer
{
    private readonly Bonus _bonus;

    public BonusTimer(Bonus bonus)
    {
        _bonus = bonus;
        SetTime(bonus.Info.Duration);
    }

    public delegate void TimerChangedHandler(Bonus bonus, float remainingSeconds, TimeChangingSource changingSource);

    public event TimerChangedHandler TimerChanged;
    public event Action<Bonus> TimerFinished;

    public bool IsActive { get; private set; }
    public bool IsPaused { get; private set; }
    public float RemainingSeconds { get; private set; }

    public void Start()
    {
        if (IsActive)
        {
            return;
        }

        IsActive = true;
        IsPaused = false;
        SubscribeOnTimeInvokerEvents();

        TimerChanged?.Invoke(_bonus, RemainingSeconds, TimeChangingSource.TimerStarted);
    }

    public void Pause()
    {
        if (IsPaused || IsActive == false)
        {
            return;
        }

        IsPaused = true;
        UnsubscribeFromTimeInvokerEvents();

        TimerChanged?.Invoke(_bonus, RemainingSeconds, TimeChangingSource.TimerPaused);
    }

    public void SetTime(float seconds)
    {
        RemainingSeconds = seconds;
        TimerChanged?.Invoke(_bonus, RemainingSeconds, TimeChangingSource.TimeForceChanged);
    }

    public void Stop()
    {
        if (IsActive == false)
        {
            return;
        }

        UnsubscribeFromTimeInvokerEvents();

        RemainingSeconds = 0f;
        IsActive = false;
        IsPaused = false;

        TimerChanged?.Invoke(_bonus, RemainingSeconds, TimeChangingSource.TimerFinished);
        TimerFinished?.Invoke(_bonus);
    }

    public void Unpause()
    {
        if (IsPaused == false || IsActive == false)
        {
            return;
        }

        IsPaused = false;
        SubscribeOnTimeInvokerEvents();

        TimerChanged?.Invoke(_bonus, RemainingSeconds, TimeChangingSource.TimerUnpaused);
    }

    private void CheckFinish()
    {
        if (RemainingSeconds <= 0f)
        {
            Stop();
        }
    }

    private void NotifyAboutTimePassed()
    {
        if (RemainingSeconds >= 0f)
        {
            TimerChanged?.Invoke(_bonus, RemainingSeconds, TimeChangingSource.TimePassed);
        }
    }

    private void OnSyncedSecondTicked()
    {
        RemainingSeconds -= 1f;

        NotifyAboutTimePassed();
        CheckFinish();
    }

    private void SubscribeOnTimeInvokerEvents()
    {
        TimeInvoker.Instance.OnOneSyncedSecondTickedEvent += OnSyncedSecondTicked;
    }

    private void UnsubscribeFromTimeInvokerEvents()
    {
        TimeInvoker.Instance.OnOneSyncedSecondTickedEvent -= OnSyncedSecondTicked;
    }
}