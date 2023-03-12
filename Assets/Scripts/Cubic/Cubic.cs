using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(Collider))]
public class Cubic : MonoBehaviour
{
    [SerializeField] private bool _canDestroy;
    [SerializeField] private VerticalSplitter _verticalSplitter;
    [SerializeField] private HorizontalSplitter _horizontalSplitter;

    private MeshRenderer _meshRenderer;
    private Collider _collider;

    public bool IsSawing { get; private set; }

    public event UnityAction Hit;

    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _collider = GetComponent<Collider>();
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
                if (trap is Saw)
                {
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
        if (other.TryGetComponent<Saw>(out Saw saw))
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
