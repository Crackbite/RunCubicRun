using System;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private ParticleSystem _effect;
    [SerializeField] private float _alphaValue = .4f;
    [SerializeField] private BlockStackRenderer _blockStackRenderer;

    public event Action<Portal> CubicEntered;

    public Color Color { get; private set; }
    public bool IsColored { get; private set; }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent(out Cubic _))
        {
            CubicEntered?.Invoke(this);
        }
    }

    public void SetColor(Color color)
    {
        Color = color;
        ParticleSystem.MainModule main = _effect.main;
        color.a = _alphaValue;
        main.startColor = color;
        IsColored = true;
    }
}