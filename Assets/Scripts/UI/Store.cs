using System;
using UnityEngine;

public class Store : MonoBehaviour
{
    [SerializeField] private ScoreAllocator _scoreAllocator;
    [SerializeField] private StoreScreen _storeScreen;
    [SerializeField] private GameStatusTracker _gameStatusTracker;
    [SerializeField] private SkinsRestorer _skinsRestorer;

    private float _currentScore;
    private bool _isFilled;

    public event Action<float> SkinBought;

    private void OnEnable()
    {
        _scoreAllocator.ScoreChanged += OnScoreChanged;
        _storeScreen.SkinChoosed += OnSkinChoosed;
        _gameStatusTracker.GameEnded += OnGameEnded;
        _gameStatusTracker.GameStarted += OnGameStarted;
    }

    private void OnDisable()
    {
        _storeScreen.SkinChoosed -= OnSkinChoosed;
        _gameStatusTracker.GameEnded -= OnGameEnded;
        _gameStatusTracker.GameStarted -= OnGameStarted;
        _scoreAllocator.ScoreChanged -= OnScoreChanged;
    }

    private void OnScoreChanged(Score score)
    {
        _currentScore = score.Current;

        if (_isFilled == false)
        {
            _storeScreen.FillScrollView(_currentScore);
            _isFilled = true;
            return;
        }

        _storeScreen.UpdateScrollView(_currentScore);
    }

    private void OnSkinChoosed(Skin skin)
    {
        if (skin.IsBought == false)
        {
            if (TrySellSkin(skin) == false)
            {
                return;
            }
        }

        foreach (Skin otherSkin in _skinsRestorer.Skins)
        {
            if (otherSkin.IsActive)
            {
                otherSkin.TurnOffActivity();
            }
        }

        skin.TurnOnActivity();
    }

    private void OnGameEnded(GameResult gameResult)
    {
        _storeScreen.UnsubscribeFromSkinView();
    }

    private void OnGameStarted()
    {
        _scoreAllocator.ScoreChanged -= OnScoreChanged;
    }

    private bool TrySellSkin(Skin skin)
    {
        const float Tolerance = 0.0001f;

        if (_currentScore - skin.Price >= -Tolerance)
        {
            skin.Buy();
            SkinBought?.Invoke(skin.Price);
            return true;
        }

        return false;
    }
}
