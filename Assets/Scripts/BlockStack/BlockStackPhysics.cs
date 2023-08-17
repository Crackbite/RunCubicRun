using UnityEngine;

public class BlockStackPhysics : MonoBehaviour
{
    [SerializeField] private Cubic _cubic;
    [SerializeField] private BlockStack _blockStack;
    [SerializeField] private float _maxPushForce = 150f;
    [SerializeField] private float _blockDestroyDelay = 5f;
    [SerializeField] private LevelExitPortal _levelExitPortal;

    private bool _isStackCollapsed;
    private Vector3 _previousPosition;
    private float _currentspeed;
    private const float MoveSpeed = 7f;

    private void OnEnable()
    {
        _blockStack.BlockAdded += OnBlockAdded;
        _cubic.Hit += OnHit;
        _levelExitPortal.SuckingIn += OnCubicSuckingIn;
    }

    private void Update()
    {
        Vector3 currentPosition = transform.position;
        Vector3 displacement = currentPosition - _previousPosition;
        Vector3 velocity = displacement / Time.deltaTime;
        _currentspeed = velocity.x;
        _previousPosition = currentPosition;
    }

    private void OnDisable()
    {
        _blockStack.BlockAdded -= OnBlockAdded;
        _cubic.Hit -= OnHit;
        _levelExitPortal.SuckingIn -= OnCubicSuckingIn;
    }

    public void OnCrossbarHit(int stackPosition)
    {
        const float ForceFactor = 0.2f;
        int brokenBlocksCount = _blockStack.Blocks.Count - stackPosition;

        _cubic.SoundSystem.Play(SoundEvent.CrossbarHit);

        for (int i = 1; i <= brokenBlocksCount; i++)
        {
            _blockStack.Blocks[0].BlockPhysics.FallOff(GetCurrentPushForce(Vector3.left), ForceFactor * (_currentspeed / MoveSpeed), true, true);
            _blockStack.AnimateDestroy(_blockStack.Blocks[0], _blockDestroyDelay);
            _blockStack.Blocks[0].BlockPhysics.CrossbarHit -= OnCrossbarHit;
        }
    }

    private void Collapse(Vector3 contactPoint, float obstacleHeight)
    {
        if (_blockStack.Blocks.Count == 0)
        {
            return;
        }
        _isStackCollapsed = true;
        Vector3 fallDirection = GetFallDirection(contactPoint, obstacleHeight);

        foreach (ColorBlock block in _blockStack.Blocks)
        {
            block.BlockPhysics.FallOff(GetCurrentPushForce(fallDirection), _currentspeed / MoveSpeed, false);
        }
    }

    private Vector3 GetCurrentPushForce(Vector3 fallDirection)
    {
        float forceMultiplier = 1f / _blockStack.Blocks.Count;

        return _maxPushForce * forceMultiplier * fallDirection;
    }

    private Vector3 GetFallDirection(Vector3 contactPoint, float obstacleHeight)
    {
        Vector3 fallDirection = contactPoint - _cubic.transform.position;
        fallDirection = new Vector3(fallDirection.x, 0f, fallDirection.z);

        if (obstacleHeight > _blockStack.Height)
        {
            return -fallDirection.normalized;
        }

        return fallDirection.normalized;
    }

    private void OnBlockAdded(ColorBlock colorBlock)
    {
        colorBlock.BlockPhysics.CrossbarHit += OnCrossbarHit;
        colorBlock.BlockPhysics.RoadHit += OnRoadHit;
    }

    private void OnRoadHit(ColorBlock colorBlock)
    {
        colorBlock.BlockPhysics.RoadHit -= OnRoadHit;
        bool isFallSoundPlaying = _cubic.SoundSystem.CheckSoundPlaying(SoundEvent.BlocksFall, out AudioSource _);
        float halfStack = _blockStack.Blocks.Count / 2;

        if (isFallSoundPlaying)
        {
            return;
        }
        else if (_isStackCollapsed)
        {
            if (colorBlock.StackPosition > halfStack)
            {
                _cubic.SoundSystem.Play(SoundEvent.BlocksFall);
            }
        }
        else
        {
            _cubic.SoundSystem.Play(SoundEvent.BlocksFall);
        }
    }

    private void OnCubicSuckingIn()
    {
        const float ForceFactor = 0.1f;

        foreach (ColorBlock block in _blockStack.Blocks)
        {
            block.BlockPhysics.FallOff(GetCurrentPushForce(Vector3.zero), ForceFactor * (_currentspeed / MoveSpeed));
        }
    }

    private void OnHit(Vector3 contactPoint, float obstacleHeight)
    {
        Collapse(contactPoint, obstacleHeight);
    }
}