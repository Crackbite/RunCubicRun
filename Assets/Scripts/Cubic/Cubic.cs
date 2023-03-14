using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(Collider))]
public class Cubic : MonoBehaviour
{
    [SerializeField] private bool _canDestroy;
    [SerializeField] private VerticalSplitter _verticalSplitter;
    [SerializeField] private HorizontalSplitter _horizontalSplitter;
    [SerializeField] private ColorHolder _colorHolder;

    private MeshRenderer _meshRenderer;
    private Collider _collider;

    public Color CurrentColor => _meshRenderer.material.color;
    public Collider Collider => _collider;
    public bool IsSawing { get; private set; }
    public bool IsSideCollision { get; private set; }
    public Trap CollisionTrap { get; private set; }

    public event UnityAction Hit;
    public event UnityAction<Color> ColorChanged;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _collider = GetComponent<Collider>();
        ChooseStartColor();
    }

    public void ChangeColor(Color color)
    {
        _meshRenderer.material.color = color;
        ColorChanged?.Invoke(color);
    }

    private void ChooseStartColor()
    {
        float minValue = -1f;
        float maxValue = _colorHolder.Colors.Count - 1;

        float random = Random.Range(minValue, maxValue);

        for (int i = 0; i < _colorHolder.Colors.Count; i++)
        {
            if (random <= i)
            {
                _meshRenderer.material.color = _colorHolder.Colors[i];
                break;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Trap>(out Trap trap))
        {
            if (_canDestroy)
            {
                trap.Break();
            }
            else
            {
                CollisionTrap = trap;
                IsSideCollision = Mathf.Abs(trap.transform.position.z - transform.position.z) >= trap.transform.localScale.z / 2;

                if (trap.TryGetComponent<Saw>(out Saw saw))
                {
                    if (saw.IsVertical == false || (saw.IsVertical && IsSideCollision == false))
                        IsSawing = true;
                }
                else
                {
                    trap.Stop();
                }

                Hit.Invoke();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Saw>(out Saw saw) && IsSawing)
        {
            if (saw.IsVertical)
                _verticalSplitter.Split();
            else
                _horizontalSplitter.Split();

            _meshRenderer.enabled = false;
            _collider.enabled = false;
        }
    }
}
