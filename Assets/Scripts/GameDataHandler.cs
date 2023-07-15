using System;
using System.Collections.Generic;
using UnityEngine;

public class GameDataHandler : MonoBehaviour
{
    [SerializeField] private ScoreAllocator _scoreAllocator;
    [SerializeField] private GameStatusTracker _gameStatusTracker;
    [SerializeField] private List<Skin> _skins;
    [SerializeField] private bool _deletePlayerPrefs;

    private const string ScoreKey = nameof(ScoreKey);
    private const string LevelKey = nameof(LevelKey);
    private const string ActiveKey = nameof(ActiveKey);
    private const string BoughtKey = nameof(BoughtKey);
    private float _score;
    private int _level = 1;
    private bool _isActiveSkinChoosed;
    private bool _isSkinBought;

    public event Action DataRestored;

    public float Score => _score;
    public int Level => _level;
    public IReadOnlyList<Skin> Skins => _skins;

    private void OnValidate()
    {
        if (_deletePlayerPrefs)
        {
            PlayerPrefs.DeleteAll();
        }
    }

    private void OnEnable()
    {
        _gameStatusTracker.GameEnded += OnGameEnded;
    }

    private void Start()
    {
        _score = TryRestoreData(ScoreKey, _score);
        _level = TryRestoreData(LevelKey, _level);

        foreach (Skin skin in _skins)
        {
            skin.ActivityChanged += OnSkinActivityChanged;
            skin.Bought += OnSkinBought;

            if (TryRestoreData(BoughtKey, skin.ID))
            {
                skin.Buy();
            }

            if (TryRestoreData(ActiveKey, skin.ID) && _isActiveSkinChoosed == false)
            {
                skin.TurnOnActivity();
                _isActiveSkinChoosed = true;
            }
            else
            {
                skin.TurnOffActivity();
            }
        }

        if (_isActiveSkinChoosed == false)
        {
            _skins[0].Buy();
            _skins[0].TurnOnActivity();
        }

        DataRestored?.Invoke();
    }

    private void OnDisable()
    {
        _gameStatusTracker.GameEnded -= OnGameEnded;

        foreach (Skin skin in _skins)
        {
            skin.ActivityChanged -= OnSkinActivityChanged;
            skin.Bought -= OnSkinBought;
        }
    }

    private float TryRestoreData(string key, float defaultResult)
    {
        if (PlayerPrefs.HasKey(key))
        {
            return PlayerPrefs.GetFloat(key);
        }

        return defaultResult;
    }

    private int TryRestoreData(string key, int defaultResult)
    {
        if (PlayerPrefs.HasKey(key))
        {
            return PlayerPrefs.GetInt(key);
        }

        return defaultResult;
    }

    private bool TryRestoreData(string key, string skinID)
    {
        if (PlayerPrefs.HasKey(skinID + key))
        {
            return Convert.ToBoolean(PlayerPrefs.GetInt(skinID + key));
        }

        return false;
    }


    private void OnGameEnded(GameResult result)
    {
        if (result == GameResult.Win)
        {
            _score = _scoreAllocator.TotalScore + _scoreAllocator.LevelScore;
            PlayerPrefs.SetFloat(ScoreKey, _score);
            _level++;
            PlayerPrefs.SetInt(LevelKey, _level);
        }
        else if (_isSkinBought)
        {
            PlayerPrefs.SetFloat(ScoreKey, _scoreAllocator.TotalScore);
        }
    }

    private void OnSkinBought(Skin skin)
    {
        PlayerPrefs.SetInt(skin.ID + BoughtKey, Convert.ToInt32(true));
        _isSkinBought = true;
    }

    private void OnSkinActivityChanged(Skin skin)
    {
        PlayerPrefs.SetInt(skin.ID + ActiveKey, Convert.ToInt32(skin.IsActive));
    }
}
