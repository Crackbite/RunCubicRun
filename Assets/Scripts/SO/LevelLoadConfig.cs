using UnityEngine;

[CreateAssetMenu(fileName = "LevelLoadConfig", menuName = "LevelData/LevelLoadConfig", order = 51)]
public class LevelLoadConfig : ScriptableObject
{
    [SerializeField] private bool _isStartWithoutMenu;
    [SerializeField] private bool _playerHasAuth;

    public bool IsStartWithoutMenu => _isStartWithoutMenu;
    public bool PlayerHasAuth => _playerHasAuth;

    public void SetStartWithoutMenu()
    {
        _isStartWithoutMenu = true;
    }

    public void SetPlayerHasAuth()
    {
        _playerHasAuth = true;
    }

    public void ResetConfig()
    {
        _isStartWithoutMenu = false;
        _playerHasAuth = false;
    }
}
