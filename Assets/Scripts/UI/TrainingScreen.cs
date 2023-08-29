using DG.Tweening;
using UnityEngine;
using static DG.Tweening.DOTweenAnimation;

public class TrainingScreen : Screen
{
    [SerializeField] private RectTransform _window;
    [SerializeField] private Vector2 _windowOnPressPosition;

    public void ChangeWindowAnimation()
    {
        _window.anchoredPosition = _windowOnPressPosition;

        if (TweenAnimations != null && TweenAnimations.Count > 0)
        {
            foreach (DOTweenAnimation doTweenAnimation in TweenAnimations)
            {
                if (doTweenAnimation.animationType == AnimationType.Move)
                {
                    doTweenAnimation.DOKill();
                    doTweenAnimation.endValueV3.y = 0;
                    doTweenAnimation.CreateTween();
                }
            }
        }

        Vector3 currentRotation = _window.rotation.eulerAngles;
        currentRotation.z = 0;
        _window.rotation = Quaternion.Euler(currentRotation);
    }

}
