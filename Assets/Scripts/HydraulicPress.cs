using UnityEngine;

public class HydraulicPress : MonoBehaviour
{
    [SerializeField] private BlocksContainer _blocksContainer;
    [SerializeField] private PressTop _pressTop;
    [SerializeField] private float _pressTopInitOffset = 10f;
    [SerializeField] private CubicMovement _cubicMovement;

    private void OnEnable()
    {
        _cubicMovement.CubicOnStand += CubicOnStand;
    }

    private void Start()
    {
        _pressTop.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        _cubicMovement.CubicOnStand -= CubicOnStand;
    }

    private void CubicOnStand()
    {
        ColorBlock highestBlock = _blocksContainer.GetBlockByIndex(0);
        float highestBlockHeight = highestBlock.transform.position.y;

        Vector3 currentPosition = _pressTop.transform.position;
        var newPosition = new Vector3(currentPosition.x, highestBlockHeight + _pressTopInitOffset, currentPosition.z);
        _pressTop.transform.position = newPosition;

        _pressTop.gameObject.SetActive(true);
        _pressTop.EnableFallAnimation(highestBlockHeight);
    }
}