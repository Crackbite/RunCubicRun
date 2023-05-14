using System;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private ParticleSystem _effect;
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
        }
    }

    public void SetColor(Color color)
    {
        if (Color == _initialColor)
        {
            _originalColor = color;
        }

        Color = color;
        ParticleSystem.MainModule main = _effect.main;
        color.a = _alphaValue;
        main.startColor = color;
        IsColored = true;
    }

    public void SetOriginalColor()
    {
        SetColor(_originalColor);
    }
}