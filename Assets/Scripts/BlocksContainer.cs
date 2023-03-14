using System;
using System.Collections.Generic;
using UnityEngine;

public class BlocksContainer : MonoBehaviour
{
    [SerializeField] private BlockStacker _blockStacker;
    [SerializeField] private Cubic _cubic;
    [SerializeField] private Material _collectedBlocksMaterial;

    private readonly List<ColorBlock> _blocks = new();

    private void OnEnable()
    {
        _blockStacker.ColorBlockAdded += OnColorBlockAdded;
        _cubic.Hit += OnHit;
        _cubic.ColorChanged += OnColorChanged;
    }

    private void Start()
    {
        _collectedBlocksMaterial.color = _cubic.CurrentColor;
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
        _cubic.ColorChanged -= OnColorChanged;
    }

    private void OnColorChanged(Color color)
    {
        foreach (var block in _blocks)
        {
            block.ChangeColor(color);
        }
    }

    private void OnHit()
    {
        Collapse();
    }

    private void OnColorBlockAdded(ColorBlock colorBlock)
    {
        _blocks.Add(colorBlock);
        colorBlock.ChangeMaterial(_collectedBlocksMaterial);
        colorBlock.EnableFollow(_blockStacker.transform);

        for (int i = 0; i < _blocks.Count; i++)
        {
            _blocks[i].StackPosition = _blocks.Count - i;
        }
    }

    private void Collapse()
    {
        Vector3 fallDirection = Vector3.right;
        Vector3 trapPosition = _cubic.CollisionTrap.transform.position;

        if (_cubic.IsSideCollision)
            fallDirection = trapPosition.z > _cubic.transform.position.z ? fallDirection + Vector3.forward : fallDirection + Vector3.back;

        for (int i = 0; i < _blocks.Count; i++)
        {
            _blocks[i].FallOff(fallDirection);
        }
    }
}