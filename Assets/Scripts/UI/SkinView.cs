using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinView : MonoBehaviour
{
    [SerializeField] private List<SkinStateForm> _skinStateForms;
    [SerializeField] private Image _icon;
    [SerializeField] private Button _choose;

    private Skin _skin;
    private float _currentScore;
    private SkinStateForm _currentStateForm;

    public event Action<Skin, SkinView> ChooseButtonClick;

    private void OnEnable()
    {
        _choose.onClick.AddListener(OnChooseClick);
    }

    private void OnDisable()
    {
        _choose.onClick.RemoveListener(OnChooseClick);
    }

    public void UnsubscribeFromSkin()
    {
        _skin.ActivityChanged -= OnSkinActivityChanged;
    }

    public void Init(Skin skin, float score)
    {
        _currentScore = score;
        _skin = skin;
        _skin.ActivityChanged += OnSkinActivityChanged;
        Render();
    }

    public void UpdateState(float currentScore)
    {
        _currentScore = currentScore;
        Render();
    }

    private void Render()
    {
        const float Tolerance = 0.0001f;

        _icon.sprite = _skin.Icon;

        if (_skin.IsBought == false)
        {
            if (_currentScore - _skin.Price >= -Tolerance)
            {
                SetSkinStateForm(SkinState.Affordable);
            }
            else
            {
                SetSkinStateForm(SkinState.Unaffordable);
            }
        }
        else if (_skin.IsActive)
        {
            SetSkinStateForm(SkinState.Selected);
        }
        else
        {
            SetSkinStateForm(SkinState.Unselected);
        }
    }

    private void SetSkinStateForm(SkinState skinState)
    {
        foreach (SkinStateForm form in _skinStateForms)
        {
            form.TurnOffActivity();
        }

        foreach (SkinStateForm form in _skinStateForms)
        {
            if (form.State == skinState)
            {
                form.Set(_skin.Price.ToString());
                _currentStateForm = form;
                return;
            }
        }
    }

    private void OnSkinActivityChanged(Skin skin)
    {
        Render();
    }

    private void OnChooseClick()
    {
        if (_currentStateForm.State == SkinState.Selected || _currentStateForm.State == SkinState.Unaffordable)
        {
            return;
        }
 
        ChooseButtonClick?.Invoke(_skin, this);
    }
}

