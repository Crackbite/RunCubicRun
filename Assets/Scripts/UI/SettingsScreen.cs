using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingsScreen : Screen
{
    [SerializeField] private Button _close;

    public event Action CloseClicked;
    public event Action Showing;

    private void OnEnable()
    {
        _close.onClick.AddListener(OnCloseClicked);
    }

    private void OnDisable()
    {
        _close.onClick.RemoveListener(OnCloseClicked);
    }

    public override void Enter()
    {
        Showing?.Invoke();
        base.Enter();
    }

    private void OnCloseClicked()
    {
        CloseClicked?.Invoke();
    }
}