using DG.Tweening;
using System;

using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class SwitchToggle : MonoBehaviour
{
    [SerializeField] private float _animationDuration = 0.2f;
    [SerializeField] private RectTransform _switcherRectTransform;
    [SerializeField] private SwitchToggleType _type;
    [SerializeField] private GameDataHandler _gameDataHandler;

    private Vector2 _handlePosition;
    private Toggle _toggle;

    public event Action<bool, SwitchToggle> ToggleChanged;

    public SwitchToggleType Type => _type;

    private void Awake()
    {
        _toggle = GetComponent<Toggle>();
        _handlePosition = _switcherRectTransform.anchoredPosition;

        if (_type == SwitchToggleType.Music)
        {
            ChangeHandlePosition(_gameDataHandler.IsMusicOn);
        }
        else
        {
            ChangeHandlePosition(_gameDataHandler.IsSoundOn);
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

    private void ChangeHandlePosition(bool isOn)
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

public enum SwitchToggleType
{
    Music,
    Sound
}