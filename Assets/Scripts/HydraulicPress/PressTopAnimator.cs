using DG.Tweening;
using UnityEngine;

public class PressTopAnimator : MonoBehaviour
{
    [SerializeField] private BlockStack _blockStack;
    [SerializeField] private float _fallSpeed = .3f;
    [SerializeField] private float _divergenceSpeed = .1f;
    [SerializeField] private float _initDivergenceOffset = .2f;
    [SerializeField] private float _stepDivergenceOffset = .01f;
    [SerializeField] private Ease _fallEase = Ease.InOutFlash;

    public void StartFallAnimation()
    {
        ColorBlock highestBlock = _blockStack.Blocks[0];
        float highestBlockY = highestBlock.GetComponent<Collider>().bounds.max.y;

        transform.DOMoveY(highestBlockY, _fallSpeed).SetEase(_fallEase).OnComplete(StartDivergenceAnimation);
    }

    private void StartDivergenceAnimation()
    {
        float zOffset = _initDivergenceOffset;

        for (int i = 0; i < _blockStack.Blocks.Count - 1; i++)
        {
            ColorBlock colorBlock = _blockStack.Blocks[i];

            var strength = new Vector3(zOffset, 0f, zOffset);
            colorBlock.transform.DOShakePosition(
                _divergenceSpeed,
                strength,
                randomnessMode: ShakeRandomnessMode.Harmonic);

            zOffset -= _stepDivergenceOffset;
            zOffset = zOffset < 0 ? 0 : zOffset;
        }
    }
}