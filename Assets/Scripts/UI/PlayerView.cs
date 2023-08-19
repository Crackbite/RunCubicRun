using TMPro;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private TMP_Text _number;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _score;

    public void Render(int number, string name, int score)
    {
        _number.text = number.ToString();
        _name.text = name;
        _score.text = score.ToString();
    }
}
