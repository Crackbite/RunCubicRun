using UnityEngine;

public class HydraulicPress : MonoBehaviour
{
    [SerializeField] private BlockStack _blockStack;
    [SerializeField] private PressTop _pressTop;
    [SerializeField] private float _pressTopInitOffset = 10f;
    [SerializeField] private CameraChanger _cameraChanger;
    [SerializeField] private BarometerArrow _barometerArrow;

    private void OnEnable()
    {
        _cameraChanger.CubicPressBlendFinished += OnCubicPressBlendFinished;
    }

    private void Start()
    {
        _pressTop.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        _cameraChanger.CubicPressBlendFinished -= OnCubicPressBlendFinished;
    }

    private void InitPress()
    {
        ColorBlock highestBlock = _blockStack.Blocks[0];
        float highestBlockY = highestBlock.GetComponent<Collider>().bounds.max.y;

        Vector3 currentPosition = _pressTop.transform.position;
        var newPosition = new Vector3(currentPosition.x, highestBlockY + _pressTopInitOffset, currentPosition.z);
        _pressTop.transform.position = newPosition;

        _pressTop.gameObject.SetActive(true);
        _pressTop.Init();
        _barometerArrow.Init();
    }

    private void OnCubicPressBlendFinished()
    {
        InitPress();
    }
}