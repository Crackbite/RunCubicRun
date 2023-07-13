using UnityEngine;

public class ChunkVisibilitySwitcher : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private float _offset = 1f;
    [SerializeField] private ChunkGenerator _chunkGenerator;

    private Chunk[] _chunks;
    private float[] _leftEdges;
    private float[] _rightEdges;

    private void OnEnable()
    {
        _chunkGenerator.Completed += OnGenerationCompleted;
    }

    private void OnDisable()
    {
        _chunkGenerator.Completed -= OnGenerationCompleted;
    }
    private void DisableChunkAbroadScreen()
    {
        float camWidth = _mainCamera.orthographicSize * 2f * _mainCamera.aspect;
        float camLeft = _mainCamera.transform.position.x - camWidth / 2f - _offset;
        float camRight = _mainCamera.transform.position.x + camWidth / 2f + _offset;

        for (int i = 0; i < _chunks.Length; i++)
        {
            bool isVisible = _rightEdges[i] > camLeft && _leftEdges[i] < camRight;
            _chunks[i].gameObject.SetActive(isVisible);
        }
    }

    private void DefineEdges()
    {
        for (int i = 0; i < _chunks.Length; i++)
        {
            var meshRenderer = _chunks[i].GetComponentInChildren<Road>().GetComponent<MeshRenderer>();
            Bounds roadBounds = meshRenderer.bounds;
            _leftEdges[i] = roadBounds.min.x;
            _rightEdges[i] = roadBounds.max.x;
        }
    }

    private void OnGenerationCompleted()
    {
        const float Dilay = 0f;
        const float RepeatRate = 0.25f;

        _chunks = GetComponentsInChildren<Chunk>();
        _leftEdges = new float[_chunks.Length];
        _rightEdges = new float[_chunks.Length];
        DefineEdges();
        InvokeRepeating(nameof(DisableChunkAbroadScreen), Dilay, RepeatRate);
    }
}

