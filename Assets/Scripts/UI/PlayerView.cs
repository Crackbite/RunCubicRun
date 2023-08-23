using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private TMP_Text _number;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _score;
    [SerializeField] private Image _bestPlayerFrame;
    [SerializeField] private Color _firstPlaceFrameColor;
    [SerializeField] private Color _secondPlaceFrameColor;
    [SerializeField] private Color _thirdPlaceFrameColor;

    public void Render(int number, string name, int score)
    {
        _number.text = number.ToString();
        _name.text = name;
        _score.text = score.ToString();

        SetFrameColor(number);
    }

    private void SetFrameColor(int placeNumber)
    {
        const float MinAlpha = 0;
        const int FirstPlace = 1;
        const int SecondPlace = 2;
        const int ThirdPlace = 3;

        Color defaultFrameColor = _bestPlayerFrame.color;
        defaultFrameColor.a = MinAlpha;

        switch (placeNumber)
        {
            case FirstPlace:
                _bestPlayerFrame.color = _firstPlaceFrameColor;
                break;
            case SecondPlace:
                _bestPlayerFrame.color = _secondPlaceFrameColor;
                break;
            case ThirdPlace:
                _bestPlayerFrame.color = _thirdPlaceFrameColor;
                break;
            default:
                _bestPlayerFrame.color = defaultFrameColor;
                break;
        }
    }
}
