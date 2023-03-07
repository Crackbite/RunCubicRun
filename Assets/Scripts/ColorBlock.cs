using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]

public class ColorBlock : MonoBehaviour
{
    [SerializeField] private float _followSpeed = 350f;

    private bool _follow;
    private Transform _followed;
    private MeshRenderer _meshRenderer;
    private float _heightPosition;

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
             Mathf.Lerp(transform.position.z, _followed.position.z, _followSpeed / _heightPosition * Time.deltaTime));
    }

    public void EnableFollow(Transform followed)
    {
        _followed = followed;
        _follow = true;
    }

    public void SetHeightPosition(float newHeightPosition)
    {
        _heightPosition = newHeightPosition;
    }
}