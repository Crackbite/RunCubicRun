using System.Collections.Generic;
using UnityEngine;

public class BlocksContainer : MonoBehaviour
{
    [SerializeField] private BlockStacker _blockStacker;
    [SerializeField] private Cubic _cubic;
    [SerializeField] private Material _stackMaterial;

    private readonly List<ColorBlock> _blocks = new();

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
        _stackMaterial.color = color;
    }

    private void OnHit()
    {
        Collapse();
    }

    public void OnColorBlockAdded(ColorBlock colorBlock)
    {
        _blocks.Add(colorBlock);
        colorBlock.PlaceInStack(_stackMaterial, _cubic, _blockStacker.Gap);
    }

    private void Collapse()
    {
        Vector3 fallDirection = Vector3.right;
        Vector3 trapPosition = Vector3.zero;

        if(_cubic.CollisionTrap != null)
        {
            trapPosition = _cubic.CollisionTrap.transform.position;
        }

        if (_cubic.IsSideCollision)
        {
            fallDirection = trapPosition.z > _cubic.transform.position.z ? fallDirection + Vector3.forward : fallDirection + Vector3.back;
        }

        for (int i = 0; i < _blocks.Count; i++)
        {
            _blocks[i].FallOff(fallDirection);
        }
    }
}