using UnityEngine;

public class PlayerPrefsDataSaver : DataSaver
{
    private const string PlayerDataKey = nameof(PlayerDataKey);

    protected override void SaveToStorage(string data)
    {
        PlayerPrefs.SetString(PlayerDataKey, data);
    }
}
