using System.Collections.Generic;
using UnityEngine;

public class SkinsRestorer : MonoBehaviour
{
    [SerializeField] private List<Skin> _skins;
    [SerializeField] private DataStorageSelector _dataStorageSelector;

    private bool _isActiveSkinChoosed;

    public IReadOnlyList<Skin> Skins => _skins;

    private void OnEnable()
    {
        _dataStorageSelector.StorageSelected += OnDataStorageSelected;
    }

    private void OnDisable()
    {
        _dataStorageSelector.StorageSelected -= OnDataStorageSelected;
    }

    public void RestoreSkinStateInfos(PlayerData playerData)
    {
        foreach (Skin skin in _skins)
        {
            foreach (SkinStateInfo info in playerData.SkinStateInfos)
            {
                if (skin.ID == info.ID)
                {
                    if (info.IsActive)
                    {
                        if (_isActiveSkinChoosed)
                        {
                            skin.StateInfo.Init(info.IsActive == false, info.IsBought);
                        }
                        else
                        {
                            skin.StateInfo.SetIsBought(info.IsBought);
                            skin.TurnOnActivity();
                            _isActiveSkinChoosed = true;
                        }
                    }
                    else
                    {
                        skin.StateInfo.Init(info.IsActive, info.IsBought);
                    }
                }
            }
        }

        TryChooseDefoultSkin();
        playerData.SetSkinsStateInfos(Skins);
    }

    private void TryChooseDefoultSkin()
    {
        if (_isActiveSkinChoosed == false)
        {
            _skins[0].StateInfo.AssignDefoult();
        }
    }

    private void OnDataStorageSelected(DataSaver dataSaver)
    {
        foreach (Skin skin in _skins)
        {
            dataSaver.SubscribeToSkinChanges(skin);
        }
    }
}
