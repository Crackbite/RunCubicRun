using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

public class ChunkStorage : MonoBehaviour
{
    [SerializeField] private string _chunksDebug;
    [SerializeField] private DataRestorer _dataRestorer;

    private List<ChunkData> _chunks = new List<ChunkData>();

    public event Action<PlayerData> Initialized;

    public static ChunkStorage Instance { get; private set; }

    public IReadOnlyList<ChunkData> Chunks => _chunks;

    private void OnEnable()
    {
        _dataRestorer.DataRestored += OnDataRestored;
    }

    private void OnDisable()
    {
        _dataRestorer.DataRestored -= OnDataRestored;
    }

    public void Add(ChunkData chunkData)
    {
        if (chunkData != null && string.IsNullOrEmpty(chunkData.Name) == false)
        {
            _chunks.Add(chunkData);
        }
    }

    public string GetChunksData()
    {
        var sb = new StringBuilder(_chunks.Count);

        for (int i = 0; i < _chunks.Count; i++)
        {
            string chunkName = $"{_chunks[i].Name.Replace("(Clone)", string.Empty)}";
            float chunkRotationAngle = _chunks[i].Rotation.eulerAngles.y;

            string text = $"{chunkName} [{chunkRotationAngle}]";
            sb.Append(i == _chunks.Count - 1 ? text : $"{text} - ");
        }

        return sb.ToString();
    }

    public void Restart()
    {
        _chunks = new List<ChunkData>();
    }

    private void Initialize(PlayerData playerData)
    {
        if (Instance == null)
        {
            foreach (ChunkData chunk in playerData.Chunks)
            {
                _chunks.Add(chunk);
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            Restart();

            if (string.IsNullOrEmpty(_chunksDebug) == false)
            {
                DebugChunks();
            }

            Initialized?.Invoke(playerData);
        }
        else
        {
            Initialized?.Invoke(playerData);
            Destroy(gameObject);
        }
    }

    private void DebugChunks()
    {
        Debug.LogWarning("Chunks Debug");

        MatchCollection matches = Regex.Matches(_chunksDebug, @"(Chunk.*?)\ \[(.*?)\]");

        foreach (Match match in matches)
        {
            string chunkName = match.Groups[1].Value;
            float chunkRotationAngle = float.Parse(match.Groups[2].Value);

            Quaternion rotation = Quaternion.Euler(0f, chunkRotationAngle, 0f);
            var chunkData = new ChunkData(chunkName, rotation);

            _chunks.Add(chunkData);
        }
    }

    private void OnDataRestored(PlayerData playerData)
    {
        Initialize(playerData);
    }
}