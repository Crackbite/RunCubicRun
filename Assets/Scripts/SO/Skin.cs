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
    [SerializeField] private SkinStateInfo _stateInfo;

    public event Action<Skin> ActivityChanged;
    public event Action<Skin> Bought;

    public string ID => _stateInfo.ID;
    public float Price => _price;
    public Sprite Icon => _icon;
    public bool IsActive => _stateInfo.IsActive;
    public bool IsBought => _stateInfo.IsBought;
    public SkinStateInfo StateInfo => _stateInfo;
    public Mesh HalfSkinMesh => _halfSkinMesh;
    public IReadOnlyList<Material> Materials => _materials;

    public void Buy()
    {
        _stateInfo.SetIsBought(true);
        Bought?.Invoke(this);
    }

    public void TurnOnActivity()
    {
        _stateInfo.SetIsActive(true);
        ActivityChanged?.Invoke(this);
    }

    public void TurnOffActivity()
    {
        _stateInfo.SetIsActive(false);
        ActivityChanged?.Invoke(this);
    }
}
