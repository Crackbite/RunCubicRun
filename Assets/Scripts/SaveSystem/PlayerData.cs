using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData
{
    [SerializeField] private float _score;
    [SerializeField] private int _leaderboardScore;
    [SerializeField] private int _level;
    [SerializeField] private int _trainingStage = 1;
    [SerializeField] private bool _isMusicOn;
    [SerializeField] private bool _isSoundOn;
    [SerializeField] private List<SkinStateInfo> _skinStateInfos = new List<SkinStateInfo>();
    [SerializeField] private List<ChunkData> _chunks = new List<ChunkData>();

    public float Score => _score;
    public int LeaderboardScore => _leaderboardScore;
    public int Level => _level;
    public int TrainingStage => _trainingStage != 0 ? _trainingStage : 1;
    public bool IsMusicOn => _isMusicOn;
    public bool IsSoundOn => _isSoundOn;
    public IReadOnlyList<SkinStateInfo> SkinStateInfos => _skinStateInfos;
    public IReadOnlyList<ChunkData> Chunks => _chunks;

    public PlayerData(float score, int leaderboardScore, int level, int trainingStageNumber, bool isMusicOn, bool isSoundOn)
    {
        _score = score;
        _leaderboardScore = leaderboardScore;
        _level = level;
        _trainingStage = trainingStageNumber;
        _isMusicOn = isMusicOn;
        _isSoundOn = isSoundOn;
    }

    public void SetSkinsStateInfos(IReadOnlyList<Skin> skins)
    {
        _skinStateInfos.Clear();

        foreach (Skin skin in skins)
        {
            _skinStateInfos.Add(skin.StateInfo);
        }
    }

    public void SetChunksData(IReadOnlyList<ChunkData> chunks)
    {
        _chunks.Clear();
        foreach (ChunkData chunk in chunks)
        {
            _chunks.Add(chunk);
        }
    }

    public void ResetScore(float newScore)
    {
        _score = newScore;
    }

    public void ResetSkinStates(Skin skin)
    {
        foreach (SkinStateInfo info in _skinStateInfos)
        {
            if (skin.ID == info.ID)
            {
                info.SetIsActive(skin.IsActive);
                info.SetIsBought(skin.IsBought);
            }
        }
    }

    public void ResetSettings(SettingsType type, bool isOn)
    {
        if (type == SettingsType.Music)
        {
            _isMusicOn = isOn;
        }
        else
        {
            _isSoundOn = isOn;
        }
    }
}
