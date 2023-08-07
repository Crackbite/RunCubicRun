using System;
using System.Collections.Generic;
using UnityEngine;

public class SkinsRestorer : MonoBehaviour
{
    [SerializeField] private DataSaver _dataSaver;

    private bool _isActiveSkinChoosed;

    public void Restore(IReadOnlyList<Skin> skins)
    {
        const int DefaultValue = 0;

        foreach (Skin skin in skins)
        {
            _dataSaver.SubscribeToSkinChanges(skin);

            if (Convert.ToBoolean(PlayerPrefs.GetInt(skin.ID + PlayerPrafsKeys.BoughtKey, DefaultValue)))
            {
                skin.Buy();
            }

            if (Convert.ToBoolean(PlayerPrefs.GetInt(skin.ID + PlayerPrafsKeys.ActiveKey, DefaultValue)))
            {
                if (_isActiveSkinChoosed == false)
                {
                    skin.TurnOnActivity();
                    _isActiveSkinChoosed = true;
                }
            }
            else
            {
                skin.TurnOffActivity();
            }
        }

        if (_isActiveSkinChoosed == false)
        {
            skins[0].Buy();
            skins[0].TurnOnActivity();
        }
    }
}
