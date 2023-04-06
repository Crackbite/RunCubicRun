using System;
using UnityEngine;
using UnityEngine.UI;

public class MenuScreen : Screen
{
    [SerializeField] private Button _start;

    public event Action StartClicked;

    private void OnEnable()
    {
        _start.onClick.AddListener(OnStartClicked);
    }

    private void OnDisable()
    {
        _start.onClick.RemoveListener(OnStartClicked);
    }

    private void OnStartClicked()
    {
        StartClicked?.Invoke();
    }
}