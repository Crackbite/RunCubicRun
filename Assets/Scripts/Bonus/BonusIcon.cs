using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image), typeof(DOTweenAnimation))]
public class BonusIcon : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _time;
    [SerializeField] private Color _goodBackground = new Color(0.33f, 1f, 0.41f);
    [SerializeField] private Color _badBackground = new Color(1f, 0.33f, 0.33f);

    private Image _background;

    public void Destroy()
    {
        var doTweenAnimation = GetComponent<DOTweenAnimation>();

        doTweenAnimation.DOPlayBackwards();
        doTweenAnimation.tween.OnRewind(() => Destroy(gameObject));
    }

    public void Init(BonusInfo bonusInfo, float startingTime)
    {
        _background = GetComponent<Image>();
        _background.color = bonusInfo.IsPositive ? _goodBackground : _badBackground;

        _icon.sprite = bonusInfo.Icon;

        SetTime(startingTime);
    }

    public void SetTime(float time)
    {
        _time.text = $"{time:F0} sec.";
    }
}