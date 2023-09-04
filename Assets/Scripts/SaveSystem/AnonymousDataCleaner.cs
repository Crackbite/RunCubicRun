using System.Collections.Generic;
using UnityEngine;

public class AnonymousDataCleaner : MonoBehaviour
{
    public void Clean(IReadOnlyList<SkinStateInfo> skinStateInfos)
    {
        foreach (string key in PlayerPrefsKeys.Keys)
        {
            if (key == PlayerPrefsKeys.BoughtKey || key == PlayerPrefsKeys.ActiveKey)
            {
                foreach (SkinStateInfo info in skinStateInfos)
                {
                    PlayerPrefs.DeleteKey(info.ID + key);
                }
            }
            else
            {
                PlayerPrefs.DeleteKey(key);
            }
        }
    }
}
