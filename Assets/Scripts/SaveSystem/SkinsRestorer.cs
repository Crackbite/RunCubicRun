using System;
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

    public void RestoreFromPlayerPrefs(PlayerData playerData)
    {
        const int DefaultValue = 0;

        foreach (Skin skin in _skins)
        {
            bool isActive = Convert.ToBoolean(PlayerPrefs.GetInt(skin.ID + PlayerPrefsKeys.ActiveKey, DefaultValue));
            bool isBought = Convert.ToBoolean(PlayerPrefs.GetInt(skin.ID + PlayerPrefsKeys.BoughtKey, DefaultValue));

            if (isActive)
            {
                if (_isActiveSkinChoosed)
                {
                    isActive = false;
                }
                else
                {
                    _isActiveSkinChoosed = true;
                }
            }

            skin.StateInfo.Init(isActive, isBought);
        }

        TryChooseDefoultSkin();
        playerData.SetSkinsStateInfos(Skins);
    }

    public void RestoreFromCloud(PlayerData playerData)
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
                            skin.StateInfo.Init(info.IsActive, info.IsBought);
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
