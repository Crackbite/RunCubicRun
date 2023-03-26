using System;
using System.Collections.Generic;
using UnityEngine;

public class BlocksContainer : MonoBehaviour
{
    [SerializeField] private BlockStacker _blockStacker;
    [SerializeField] private BlockMovementCoordinator _coordinator;
    [SerializeField] private Cubic _cubic;

    private readonly List<ColorBlock> _blocks = new();

    public event Action BlockRemoved;

    public int BlocksCount => _blocks.Count;
    public Color CurrentColor { get; private set; }

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

    public int GetStackPosition(ColorBlock targetBlock)
    {
        int position = 0;

        for (int i = 0; i < _blocks.Count; i++)
        {
            if (_blocks[i] == targetBlock)
            {
                position = _blocks.Count - i;
                return position;
            }
        }

        return position;
    }

    public void ChangeColor(Color color)
    {
        CurrentColor = color;

        foreach (ColorBlock block in _blocks)
        {
            block.BlockRenderer.SetColor(color);
        }
    }

    public void DestroyBlock(ColorBlock colorBlock)
    {
        Destroy(colorBlock.gameObject);
        _blocks.Remove(colorBlock);

        BlockRemoved?.Invoke();
    }

    public ColorBlock GetBlockByIndex(int index) => _blocks[index];

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

        foreach (ColorBlock block in _blocks)
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

        _blocks.Add(colorBlock);

        colorBlock.BlockPhysics.CrossbarHit += OnCrossbarHit;
        colorBlock.Init(this, _coordinator);
    }

    private void OnCrossbarHit(int stackPosition)
    {
        const float ForceFactor = 0.1f;

        int brokenBlocksCount = _blocks.Count - stackPosition;

        for (int i = 1; i <= brokenBlocksCount; i++)
        {
            _blocks[0].BlockPhysics.FallOff(Vector3.left, ForceFactor);
            _blocks.Remove(_blocks[0]);
            _blocks[0].BlockPhysics.CrossbarHit -= OnCrossbarHit;
        }
    }

    private void OnHit()
    {
        Collapse();
    }
}