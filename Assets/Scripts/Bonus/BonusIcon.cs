using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class BonusIcon : MonoBehaviour
{
    [SerializeField] private TMP_Text _time;

    private Image _image;

    public void Init(Sprite icon, float startingTime)
    {
        _image = GetComponent<Image>();
        _image.sprite = icon;

        SetTime(startingTime);
    }

    public void SetTime(float time)
    {
        _time.text = time.ToString(CultureInfo.InvariantCulture);
    }
}