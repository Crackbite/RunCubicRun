using DG.Tweening;
using UnityEngine;
using static DG.Tweening.DOTweenAnimation;

public class TrainingScreen : Screen
{
    [SerializeField] private RectTransform _window;

    private Vector3 _windowInitialPosition;

    private void Awake()
    {
        _windowInitialPosition = _window.position;
    }

    public void ChangeWindowAnimation()
    {
        _window.position = _windowInitialPosition;

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
