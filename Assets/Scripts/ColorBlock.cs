using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class ColorBlock : MonoBehaviour
{
    [SerializeField] private float _followSpeed = 150f;

    private bool _follow;
    private Transform _followed;
    private MeshRenderer _meshRenderer;

    public Material CurrentMaterial => _meshRenderer.sharedMaterial;

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

        transform.position = new Vector3(
            transform.position.x,
            transform.position.y,
            Mathf.Lerp(transform.position.z, _followed.position.z, _followSpeed * Time.deltaTime));
    }

    public void EnableFollow(Transform followed)
    {
        _followed = followed;
        _follow = true;
    }
}