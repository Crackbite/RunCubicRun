using UnityEngine;

[RequireComponent(typeof(ColorBlockCollection))]
public class BlocksContainer : MonoBehaviour
{
    [SerializeField] private Cubic _cubic;
    [SerializeField] private BlockMovementCoordinator _coordinator;

    private ColorBlockCollection _blockCollection;

    private void Awake()
    {
        _blockCollection = GetComponent<ColorBlockCollection>();
    }

    private void OnEnable()
    {
        _blockCollection.BlockAdded += OnBlockAdded;
    }

    private void Update()
    {
        Vector3 currentPosition = transform.position;
        transform.position = new Vector3(_cubic.transform.position.x, currentPosition.y, currentPosition.z);
    }

    private void OnDisable()
    {
        _blockCollection.BlockAdded -= OnBlockAdded;
    }

    public int GetStackPosition(ColorBlock targetBlock)
    {
        int position = 0;

        for (int i = 0; i < _blockCollection.Blocks.Count; i++)
        {
            if (_blockCollection.Blocks[i] == targetBlock)
            {
                position = _blockCollection.Blocks.Count - i;
                return position;
            }
        }

        return position;
    }

    public void PlaceInStack(ColorBlock colorBlock)
    {
        colorBlock.Init(this, _coordinator);
    }

    private void OnBlockAdded(ColorBlock colorBlock)
    {
        PlaceInStack(colorBlock);
    }
}