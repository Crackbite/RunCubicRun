using System;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private List<ParticleSystem> _effects;
    [SerializeField] private ParticleSystem _passageEffect;
    [SerializeField] private float _alphaValue = .4f;

    private BonusHandler _bonusHandler;
    private Color _initialColor;
    private Color _originalColor;

    public event Action<Portal> CubicEntered;

    public Color Color { get; private set; }
    public bool IsColored { get; private set; }

    private void Start()
    {
        _initialColor = Color;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent(out Cubic _))
        {
            CubicEntered?.Invoke(this);
            _passageEffect.gameObject.SetActive(true);
        }
    }

    public void Init(BonusHandler bonusHandler)
    {
        _bonusHandler = bonusHandler;
    }

    public void SetColor(Color color)
    {
        if (Color == _initialColor)
        {
            _originalColor = color;
        }

        Color = color;

        foreach (ParticleSystem effect in _effects)
        {
            ParticleSystem.MainModule main = effect.main;
            color.a = _alphaValue;
            main.startColor = color;
        }

        IsColored = true;
    }

    public void TrySetOriginalColor()
    {
        IReadOnlyDictionary<BonusInfo, BonusItem> _activeBonuses = _bonusHandler.ActiveBonuses;

        foreach (KeyValuePair<BonusInfo, BonusItem> pair in _activeBonuses)
        {
            Bonus bonus = pair.Value.Bonus;

            if (bonus != null && bonus is PortalRecolorBonus)
            {
                return;
            }
        }

        SetColor(_originalColor);
    }
}