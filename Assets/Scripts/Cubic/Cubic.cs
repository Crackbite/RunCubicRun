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

    public Collider Collider => _collider;
    public bool IsSawing { get; private set; }
    public bool IsSideCollision { get; private set; }
    public Trap CollisionTrap { get; private set; }

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
