using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreScreen : Screen
{
    [SerializeField] private DataRestorer _dataRestorer;
    [SerializeField] private SkinView _viewTemplate;
    [SerializeField] private GameObject _itemContainer;
    [SerializeField] private Button _close;

    private List<SkinView> _skinViews = new List<SkinView>();

    public event Action<Skin> SkinChoosed;
    public event Action CloseClicked;
    public event Action SkinViewsFilled;

    public IReadOnlyList<SkinView> SkinViews => _skinViews;

    private void OnEnable()
    {
        _close.onClick.AddListener(OnCloseClicked);
    }

    private void OnDisable()
    {
        _close.onClick.RemoveListener(OnCloseClicked);
    }

    public void FillScrollView(float currentScore)
    {
        IReadOnlyList<Skin> skins = _dataRestorer.Skins;

        for (int i = 0; i < skins.Count; i++)
        {
            AddSkinView(skins[i], currentScore);
        }

        SkinViewsFilled?.Invoke();
    }

    public void UpdateScrolView(float currentScore)
    {
        foreach (SkinView view in _skinViews)
        {
            view.UpdateState(currentScore);
        }
    }

    public void UnsubscribeFromSkinView()
    {
        foreach (SkinView view in _skinViews)
        {
            view.ChooseButtonClick -= OnChooseButtonClick;
            view.UnsubscribeFromSkin();
        }
    }

    private void AddSkinView(Skin skin, float currentScore)
    {
        SkinView view = Instantiate(_viewTemplate, _itemContainer.transform);
        view.ChooseButtonClick += OnChooseButtonClick;
        view.Init(skin, currentScore);
        _skinViews.Add(view);
    }

    private void OnChooseButtonClick(Skin skin, SkinView view)
    {
        SkinChoosed?.Invoke(skin);
    }

    private void OnCloseClicked()
    {
        CloseClicked?.Invoke();
    }
}
