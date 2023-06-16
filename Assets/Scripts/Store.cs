using System;
using UnityEngine;

public class Store : MonoBehaviour
{
    [SerializeField] private ScoreAllocator _scoreAllocator;
    [SerializeField] private StoreScreen _storeScreen;
    [SerializeField] private GameStatusTracker _gameStatusTracker;

    private float _currentScore;

    public event Action<float> SkinBought;

    private void OnEnable()
    {
        _scoreAllocator.ScoreChanged += OnScoreChanged;
        _storeScreen.SkinChoosed += OnSkinChoosed;
        _gameStatusTracker.GameEnded += OnGameEnded;
    }

    private void OnDisable()
    {
        _storeScreen.SkinChoosed -= OnSkinChoosed;
        _gameStatusTracker.GameEnded += OnGameEnded;
    }

    private void OnScoreChanged(Score score)
    {
        _currentScore = score.Current;
        _storeScreen.FillScrollView(_currentScore);
        _scoreAllocator.ScoreChanged -= OnScoreChanged;
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

        foreach (Skin otherSkin in _storeScreen.Skins)
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
