using Agava.YandexGames;

public class CloudDataSaver : DataSaver
{
    protected override void SaveToStorage(string data)
    {
        PlayerAccount.SetPlayerData(data);
    }

    protected override void OnGameEnded(GameResult result)
    {
        if (result == GameResult.Win)
        {
            if (ChunkStorage.Instance != null)
            {
                ChunkStorage.Instance.Restart();
            }

            Save(result);
        }
        else
        {
            SaveChunks();
        }
    }
}
