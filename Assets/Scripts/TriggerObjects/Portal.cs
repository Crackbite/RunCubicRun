using System;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private List<ParticleSystem> _effects;
    [SerializeField] private ParticleSystem _passageEffect;
    [SerializeField] private float _alphaValue = .4f;

    private Color _initialColor;
    private Color _originalColor;

    public event Action<Portal> CubicEntered;

    public Color Color { get; private set; }
    public bool IsColored { get; private set; }
    public Bonus UsedByBonus { get; set; }

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

    public void SetOriginalColor()
    {
        SetColor(_originalColor);
    }
}