using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class SwitchToggle : MonoBehaviour
{
    [SerializeField] private float _animationDuration = 0.2f;
    [SerializeField] private RectTransform _switcherRectTransform;

    private Vector2 _handlePosition;
    private Toggle _toggle;

    private void Awake()
    {
        _toggle = GetComponent<Toggle>();
        _handlePosition = _switcherRectTransform.anchoredPosition;

        ChangeHandlePosition(_toggle.isOn);
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
    }

    private void OnSwitch(bool isOn)
    {
        ChangeHandlePosition(isOn);
    }
}