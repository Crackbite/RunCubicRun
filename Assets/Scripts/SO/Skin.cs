using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSkin", menuName = "Skin", order = 51)]
public class Skin : ScriptableObject
{
    [SerializeField] private int _price;
    [SerializeField] private Sprite _icon;
    [SerializeField] private Mesh _halfSkinMesh;
    [SerializeField] private List<Material> _materials;
    [SerializeField] private bool _isActive;
    [SerializeField] private bool _isBought;

    private const string ActiveKey = nameof(ActiveKey);
    private const string BoughtKey = nameof(BoughtKey);

    public event Action<Skin> Activated;
    public event Action Deactivated;
    public event Action Bought;

    public float Price => _price;
    public Sprite Icon => _icon;
    public bool IsActive => _isActive;
    public bool IsBought => _isBought;
    public Mesh HalfSkinMesh => _halfSkinMesh;
    public IReadOnlyList<Material> Materials => _materials;

    public void Buy()
    {
        _isBought = true;
        Bought?.Invoke();
    }

    public void TurnOnActivity()
    {
        _isActive = true;
        Activated?.Invoke(this);
    }

    public void TurnOffActivity()
    {
        _isActive = false;
        Deactivated?.Invoke();
    }
}
