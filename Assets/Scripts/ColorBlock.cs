using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class ColorBlock : MonoBehaviour
{
    [SerializeField] private float _followSpeed = 350f;
    [SerializeField] private float _frictionCoefficient = 10f;

    private bool _follow;
    private Transform _followed;
    private MeshRenderer _meshRenderer;
    private Collider _collider;
    private Rigidbody _rigidbody;

    public Material CurrentMaterial => _meshRenderer.sharedMaterial;

    public float StackPosition { get; set; }

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _collider = GetComponent<Collider>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (_follow == false)
        {
            return;
        }

        Vector3 currentPosition = transform.position;
        float interpolation = _followSpeed / StackPosition * Time.deltaTime;

        transform.position = new Vector3(
            currentPosition.x,
            currentPosition.y,
            Mathf.Lerp(currentPosition.z, _followed.position.z, interpolation));
    }

    public void EnableFollow(Transform followed)
    {
        _followed = followed;
        _follow = true;
    }

    public void FallOff(Vector3 fallDirection)
    {
        enabled = false;
        _rigidbody.isKinematic = false;
        _rigidbody.AddForce(fallDirection * _frictionCoefficient * StackPosition);
        _collider.isTrigger = false;
    }

    public void ChangeColor(Color color)
    {
        CurrentMaterial.color = color;
    }

    public void ChangeMaterial(Material newMaterial)
    {
        _meshRenderer.material = newMaterial;
    }
}