using System;
using UnityEngine;

public class BlockStacker : MonoBehaviour
{
    [SerializeField] private float _gap = .02f;
    [SerializeField] private BlocksContainer _blocksContainer;

    private Vector3 _initialStackPosition;

    public event Action<Transform> BlockAdded;

    private void Start()
    {
        _initialStackPosition = _blocksContainer.transform.localPosition;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent(out ColorBlock colorBlock) == false)
        {
            return;
        }

        Transform containerTransform = _blocksContainer.transform;
        Vector3 containerPosition = containerTransform.position;
        Transform blockTransform = colorBlock.transform;

        blockTransform.SetParent(gameObject.transform);

        containerTransform.position = new Vector3(
            containerPosition.x,
            containerPosition.y + blockTransform.localScale.y + _gap,
            containerPosition.z);

        blockTransform.localPosition = _initialStackPosition;
        blockTransform.SetParent(containerTransform);

        BlockAdded?.Invoke(blockTransform);
    }
}