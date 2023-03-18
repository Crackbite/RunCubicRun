using UnityEngine;
using UnityEngine.Events;

public class Portal : MonoBehaviour
{
    [SerializeField] private ParticleSystem _effect;
    [SerializeField] private float _alphaValue;

    public Color Color { get; private set; }

    public event UnityAction<Portal> CubicEntered;

    public void SetColor(Color color)
    {
        Color = color;
        var main = _effect.main;
        color.a = _alphaValue;
        main.startColor = color;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Cubic>(out Cubic cubic))
            CubicEntered?.Invoke(this);
    }
}
