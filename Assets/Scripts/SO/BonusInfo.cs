using UnityEngine;

[CreateAssetMenu(fileName = "BonusInfo", menuName = "LevelData/BonusInfo", order = 51)]
public class BonusInfo : ScriptableObject
{
    [SerializeField] private Sprite _icon;
    [SerializeField] private string _description;
    [SerializeField] private float _duration;
    [SerializeField] private bool _isPositive;
    
    public string Description => _description;
    public float Duration => _duration;
    public Sprite Icon => _icon;
    public bool IsPositive => _isPositive;
}