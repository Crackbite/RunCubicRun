using UnityEngine;

public class PressTop : MonoBehaviour
{
    public void Init()
    {
        if (TryGetComponent(out PressTopAnimator pressTopAnimator))
        {
            pressTopAnimator.EnableFallAnimation();
        }
    }
}