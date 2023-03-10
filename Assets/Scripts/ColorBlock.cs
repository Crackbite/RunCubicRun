using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class ColorBlock : MonoBehaviour
{
    [SerializeField] private float _followSpeed = 350f;

    private bool _follow;
    private Transform _followed;
    private MeshRenderer _meshRenderer;

    public Material CurrentMaterial => _meshRenderer.sharedMaterial;

    public float StackPosition { get; set; }

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
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
}