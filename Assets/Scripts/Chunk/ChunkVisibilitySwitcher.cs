using System.Collections.Generic;
using UnityEngine;

public class ChunkVisibilitySwitcher : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private float _offset = 1f;
    [SerializeField] private LevelGenerationStarter _generatorStarter;
    [SerializeField] private AuthRequestScreen _authRequestScreen;
    [SerializeField] private Chunk _finalChunk;
    [SerializeField] private Chunk _starterChunk;

    private ChunkGenerator _currentGenerator;

    private List<Chunk> _chunks = new List<Chunk>();
    private float[] _leftEdges;
    private float[] _rightEdges;

    private void OnEnable()
    {
        _generatorStarter.GeneratorStarted += OnGeneratorStarted;
        _authRequestScreen.PlayerAuthorized += OnPlayerAuthorized;
    }

    private void OnDisable()
    {
        _generatorStarter.GeneratorStarted -= OnGeneratorStarted;
        _authRequestScreen.PlayerAuthorized -= OnPlayerAuthorized;
        _currentGenerator.Completed -= OnChunkGenerationCompleted;
    }

    private void DisableChunkAbroadScreen()
    {
        float camWidth = _mainCamera.orthographicSize * 2f * _mainCamera.aspect;
        float camLeft = _mainCamera.transform.position.x - camWidth / 2f - _offset;
        float camRight = _mainCamera.transform.position.x + camWidth / 2f + _offset;

        for (int i = 0; i < _chunks.Count; i++)
        {
            bool isVisible = _rightEdges[i] > camLeft && _leftEdges[i] < camRight;
            _chunks[i].gameObject.SetActive(isVisible);
        }
    }

    private void DefineEdges()
    {
        for (int i = 0; i < _chunks.Count; i++)
        {
            var meshRenderer = _chunks[i].GetComponentInChildren<Road>().GetComponent<MeshRenderer>();
            Bounds roadBounds = meshRenderer.bounds;
            _leftEdges[i] = roadBounds.min.x;
            _rightEdges[i] = roadBounds.max.x;
        }
    }

    private void OnGeneratorStarted(ChunkGenerator currentGenerator)
    {
        if (_currentGenerator != null)
        {
            _currentGenerator.Completed -= OnChunkGenerationCompleted;
        }

        _currentGenerator = currentGenerator;
        _currentGenerator.Completed += OnChunkGenerationCompleted;
    }

    private void OnChunkGenerationCompleted()
    {
        const float Dilay = 0f;
        const float RepeatRate = 0.25f;

        _chunks.Clear();
        _chunks.Add(_starterChunk);

        foreach (Chunk chunk in _currentGenerator.Chunks)
        {
            _chunks.Add(chunk);
        }

        _chunks.Add(_finalChunk);
        _leftEdges = new float[_chunks.Count];
        _rightEdges = new float[_chunks.Count];
        DefineEdges();
        InvokeRepeating(nameof(DisableChunkAbroadScreen), Dilay, RepeatRate);
    }

    private void OnPlayerAuthorized()
    {
        CancelInvoke(nameof(DisableChunkAbroadScreen));
    }
}

