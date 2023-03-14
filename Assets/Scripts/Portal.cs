using UnityEngine;
using UnityEngine.Events;

public class Portal : MonoBehaviour
{
    [SerializeField] private ParticleSystem _effect;
    [SerializeField] private float _alphaValue;

    private Color _color;

    public event UnityAction<Portal> CubicEntered;

    public void SetColor(Color color)
    {
        _color = color;
        var main = _effect.main;
        color.a = _alphaValue;
        main.startColor = color;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Cubic>(out Cubic cubic))
        {
            cubic.ChangeColor(_color);
            CubicEntered?.Invoke(this);
        }
    }
}
