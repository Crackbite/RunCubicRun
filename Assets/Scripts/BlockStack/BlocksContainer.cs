using UnityEngine;

[RequireComponent(typeof(ColorBlockCollection))]
public class BlocksContainer : MonoBehaviour
{
    [SerializeField] private BlockStacker _blockStacker;
    [SerializeField] private BlockMovementCoordinator _coordinator;
    [SerializeField] private Cubic _cubic;

    private ColorBlockCollection _blockCollection;

    public Color CurrentColor { get; private set; }

    private void Awake()
    {
        _blockCollection = GetComponent<ColorBlockCollection>();
    }

    private void OnEnable()
    {
        _blockStacker.ColorBlockAdded += OnColorBlockAdded;
        _cubic.Hit += OnHit;
    }

    private void Update()
    {
        Vector3 currentPosition = transform.position;
        transform.position = new Vector3(_blockStacker.transform.position.x, currentPosition.y, currentPosition.z);
    }

    private void OnDisable()
    {
        _blockStacker.ColorBlockAdded -= OnColorBlockAdded;
        _cubic.Hit -= OnHit;
    }

    public void ChangeColor(Color color)
    {
        CurrentColor = color;

        foreach (ColorBlock block in _blockCollection.Blocks)
        {
            block.BlockRenderer.SetColor(color);
        }
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

    private void Collapse()
    {
        Vector3 fallDirection = Vector3.right;
        Vector3 trapPosition = Vector3.zero;

        if (_cubic.CollisionTrap != null)
        {
            trapPosition = _cubic.CollisionTrap.transform.position;
        }

        if (_cubic.IsSideCollision)
        {
            fallDirection = trapPosition.z > _cubic.transform.position.z
                                ? fallDirection + Vector3.forward
                                : fallDirection + Vector3.back;
        }

        foreach (ColorBlock block in _blockCollection.Blocks)
        {
            block.BlockPhysics.FallOff(fallDirection);
        }
    }

    private void OnColorBlockAdded(ColorBlock colorBlock)
    {
        if (CurrentColor != colorBlock.BlockRenderer.CurrentColor)
        {
            CurrentColor = colorBlock.BlockRenderer.CurrentColor;
        }

        _blockCollection.Add(colorBlock);

        colorBlock.BlockPhysics.CrossbarHit += OnCrossbarHit;
        colorBlock.Init(this, _coordinator);
    }

    private void OnCrossbarHit(int stackPosition)
    {
        const float BlockDestroyDelay = 5f;
        const float ForceFactor = 0.1f;

        int brokenBlocksCount = _blockCollection.Blocks.Count - stackPosition;

        for (int i = 1; i <= brokenBlocksCount; i++)
        {
            _blockCollection.Blocks[0].BlockPhysics.FallOff(Vector3.left, ForceFactor);
            _blockCollection.Destroy(_blockCollection.Blocks[0], BlockDestroyDelay);
            _blockCollection.Blocks[0].BlockPhysics.CrossbarHit -= OnCrossbarHit;
        }
    }

    private void OnHit()
    {
        Collapse();
    }
}