using System;
using UnityEngine;

public class BlockStacker : MonoBehaviour
{
    [SerializeField] private float _gap = .02f;
    [SerializeField] private BlocksContainer _blocksContainer;

    private float _stackYPosition;

    public float Gap => _gap;

    public event Action<ColorBlock> ColorBlockAdded;
    public event Action WrongBlockTaken;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent(out ColorBlock colorBlock) == false || colorBlock.IsFollow)
        {
            return;
        }

        if (_blocksContainer.BlocksCount > 0 && _blocksContainer.CurrentColor != colorBlock.CurrentColor)
        {
            Destroy(colorBlock.gameObject);
            WrongBlockTaken?.Invoke();
            return;
        }


        Transform containerTransform = _blocksContainer.transform;
        Vector3 containerPosition = containerTransform.position;
        Transform blockTransform = colorBlock.transform;

        blockTransform.SetParent(gameObject.transform);

        if (_stackYPosition == 0)
        {
            _stackYPosition = colorBlock.transform.localScale.y / 2 + transform.localScale.y / 2 + _gap;
        }

        containerTransform.position = new Vector3(
            containerPosition.x,
            containerPosition.y + blockTransform.localScale.y + _gap,
            containerPosition.z);

        blockTransform.localPosition = new Vector3(0f, _stackYPosition, 0f);
        blockTransform.SetParent(containerTransform);

        ColorBlockAdded?.Invoke(colorBlock);
    }
}