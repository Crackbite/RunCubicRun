using System;
using UnityEngine;
using UnityEngine.UI;

public class MenuScreen : Screen
{
    [SerializeField] private Button _start;
    [SerializeField] private Button _store;

    public event Action StartClicked;
    public event Action StoreClicked;

    private void OnEnable()
    {
        _start.onClick.AddListener(OnStartClicked);
        _store.onClick.AddListener(OnStoreClicked);
    }

    private void OnDisable()
    {
        _start.onClick.RemoveListener(OnStartClicked);
        _store.onClick.RemoveListener(OnStoreClicked);

    }

    private void OnStartClicked()
    {
        StartClicked?.Invoke();
    }

    private void OnStoreClicked()
    {
        StoreClicked?.Invoke();
    }
}