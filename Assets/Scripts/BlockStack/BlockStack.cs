using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BlockStackAddAnimator), typeof(BlockStackDestroyAnimator))]
public class BlockStack : MonoBehaviour
{
    [SerializeField] private Cubic _cubic;
    [SerializeField] private BlockStackCoordinator _stackCoordinator;

    private readonly List<ColorBlock> _blocks = new();

    private BlockStackAddAnimator _addAnimator;
    private BlockStackDestroyAnimator _destroyAnimator;
    private float _groundPositionY = 0;

    public event Action<ColorBlock> BlockAdded;
    public event Action<ColorBlock> BlockRemoved;
    public event Action BlocksEnded;

    public IReadOnlyList<ColorBlock> Blocks => _blocks;
    public float Height => _blocks[0].transform.position.y;

    private void Awake()
    {
        _addAnimator = GetComponent<BlockStackAddAnimator>();
        _destroyAnimator = GetComponent<BlockStackDestroyAnimator>();
    }

    private void OnEnable()
    {
        _destroyAnimator.AnimationCompleted += OnDestroyAnimationCompleted;
        _cubic.Hit += OnCubicHit;
    }

    private void LateUpdate()
    {
        if(_cubic.transform.position.y < _groundPositionY)
        {
            return;
        }

        Vector3 currentPosition = transform.position;
        transform.position = new Vector3(_cubic.transform.position.x, currentPosition.y, currentPosition.z);
    }

    public void Add(ColorBlock colorBlock)
    {
        _blocks.Add(colorBlock);
        PlaceInStack(colorBlock);
        _addAnimator.StartAddAnimation(colorBlock);

        BlockAdded?.Invoke(colorBlock);
    }

    public void AnimateDestroy(ColorBlock colorBlock, float delay = 0f)
    {
        if (TryRemoveBlock(colorBlock) == false)
        {
            return;
        }

        _destroyAnimator.StartDestroyAnimation(colorBlock, delay);
    }

    public void Destroy(ColorBlock colorBlock)
    {
        if (TryRemoveBlock(colorBlock) == false)
        {
            return;
        }

        Destroy(colorBlock.gameObject);
    }

    public int GetStackPosition(ColorBlock targetBlock)
    {
        const int DefaultPosition = 0;

        for (int i = 0; i < _blocks.Count; i++)
        {
            if (_blocks[i] == targetBlock)
            {
                return _blocks.Count - i;
            }
        }

        return DefaultPosition;
    }

    public void PlaceInStack(ColorBlock colorBlock)
    {
        colorBlock.Init(this, _stackCoordinator);
    }

    private void OnDestroyAnimationCompleted(ColorBlock colorBlock)
    {
        Destroy(colorBlock.gameObject);
    }


    private void OnCubicHit(Vector3 contactPoint, float obstacleHeight)
    {
        if (_cubic.IsSawing == false)
        {
            foreach (ColorBlock block in _blocks)
            {
                block.transform.parent = null;
            }
        }
    }

    private bool TryRemoveBlock(ColorBlock colorBlock)
    {
        if (_blocks.Contains(colorBlock) == false)
        {
            return false;
        }

        _blocks.Remove(colorBlock);
        BlockRemoved?.Invoke(colorBlock);

        if (_blocks.Count < 1)
        {
            BlocksEnded?.Invoke();
        }

        return true;
    }
}