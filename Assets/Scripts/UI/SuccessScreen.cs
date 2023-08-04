using UnityEngine;
using UnityEngine.UI;

public class SuccessScreen : LevelResultScreen
{
    [SerializeField] private Button _next;

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

    private void OnNextClicked()
    {
        if (ChunkStorage.Instance != null)
        {
            ChunkStorage.Instance.Restart();
        }

        LoadScene();
    }
}