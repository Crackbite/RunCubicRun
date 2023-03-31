using System;
using UnityEngine;

public class BlockStacker : MonoBehaviour
{
    [SerializeField] private float _gap = .02f;
    [SerializeField] private BlockStackRenderer _blockStackRenderer;
    [SerializeField] private BlockStack _blockStack;

    private bool _blocksEnded;
    private float _stackYPosition;

    public event Action WrongBlockTaken;

    private void OnEnable()
    {
        _blockStack.BlocksEnded += OnBlocksEnded;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent(out ColorBlock colorBlock) == false || colorBlock.CanFollow || _blocksEnded)
        {
            return;
        }

        if (_blockStackRenderer.IsColorAssigned && _blockStackRenderer.CurrentColor != colorBlock.BlockRenderer.CurrentColor)
        {
            Destroy(colorBlock.gameObject);
            _blockStack.AnimateDestroy(_blockStack.Blocks[0]);

            WrongBlockTaken?.Invoke();
            return;
        }

        Transform containerTransform = _blockStackRenderer.transform;
        Vector3 containerPosition = containerTransform.position;
        Transform blockTransform = colorBlock.transform;

        blockTransform.SetParent(gameObject.transform);

        if (_stackYPosition == 0)
        {
            _stackYPosition = (colorBlock.transform.localScale.y / 2f) + (transform.localScale.y / 2f) + _gap;
        }

        containerTransform.position = new Vector3(
            containerPosition.x,
            containerPosition.y + blockTransform.localScale.y + _gap,
            containerPosition.z);

        blockTransform.localPosition = new Vector3(0f, _stackYPosition, 0f);
        blockTransform.SetParent(containerTransform);

        _blockStack.Add(colorBlock);
    }

    private void OnBlocksEnded()
    {
        _blocksEnded = true;
    }
}