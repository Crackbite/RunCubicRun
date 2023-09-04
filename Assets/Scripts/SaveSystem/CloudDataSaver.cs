using Agava.YandexGames;

public class CloudDataSaver : DataSaver
{
    protected override void SaveToStorage(string data)
    {
        PlayerAccount.SetPlayerData(data);
    }
}
