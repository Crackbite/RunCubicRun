using UnityEngine;

[CreateAssetMenu(fileName = "LevelLoadConfig", menuName = "LevelData/LevelLoadConfig", order = 51)]
public class LevelLoadConfig : ScriptableObject
{
    [SerializeField] private bool _isStartWithoutMenu;

    public bool IsStartWithoutMenu => _isStartWithoutMenu;

    public void SetStartWithoutMenu()
    {
        _isStartWithoutMenu = true;
    }

    public void ResetConfig()
    {
        _isStartWithoutMenu = false;
    }
}
