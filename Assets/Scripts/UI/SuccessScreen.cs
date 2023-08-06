using System;
using UnityEngine;
using UnityEngine.UI;

public class SuccessScreen : LevelResultScreen
{
    [SerializeField] private Button _next;

    public event Action NextLevelLoading;

    protected override void OnEnable()
    {
        base.OnEnable();
        _next.onClick.AddListener(OnNextClicked);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _next.onClick.RemoveListener(OnNextClicked);
    }

    protected override void RestartLevel()
    {
        NextLevelLoading?.Invoke();
    }

    private void OnNextClicked()
    {
        if (ChunkStorage.Instance != null)
        {
            ChunkStorage.Instance.Restart();
        }

        LoadScene();
    }
}