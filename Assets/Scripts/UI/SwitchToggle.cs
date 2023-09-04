using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class SwitchToggle : MonoBehaviour
{
    [SerializeField] private float _animationDuration = 0.2f;
    [SerializeField] private RectTransform _switcherRectTransform;
    [SerializeField] private SettingsType _type;
    [SerializeField] private DataRestorer _dataRestorer;
    [SerializeField] private Toggle _toggle;

    private Vector2 _handlePosition;

    public event Action<bool, SwitchToggle> ToggleChanged;

    public SettingsType Type => _type;

    private void Awake()
    {
        if (_type == SettingsType.Music)
        {
            ChangeHandlePosition(_dataRestorer.PlayerData.IsMusicOn);
        }
        else
        {
            ChangeHandlePosition(_dataRestorer.PlayerData.IsSoundOn);
        }
    }

    private void OnEnable()
    {
        _toggle.onValueChanged.AddListener(OnSwitch);
    }

    private void OnDisable()
    {
        _toggle.onValueChanged.RemoveListener(OnSwitch);
    }

    public void SetHandlePosition()
    {
        _handlePosition = _switcherRectTransform.anchoredPosition;
    }

    public void ChangeHandlePosition(bool isOn)
    {
        Vector2 newHandlePosition = isOn ? _handlePosition * -1 : _handlePosition;
        _switcherRectTransform.DOAnchorPosX(newHandlePosition.x, _animationDuration).SetUpdate(true);
        _toggle.isOn = isOn;
        ToggleChanged?.Invoke(isOn, this);
    }

    private void OnSwitch(bool isOn)
    {
        ChangeHandlePosition(isOn);
    }
}

public enum SettingsType
{
    Music,
    Sound
}