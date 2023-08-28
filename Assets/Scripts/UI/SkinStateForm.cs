using TMPro;
using UnityEngine;

[System.Serializable]
public class SkinStateForm
{
    [SerializeField] private GameObject _form;
    [SerializeField] private TMP_Text _tag;
    [SerializeField] private SkinState _state;

    public SkinState State => _state;

    public void Set(string price)
    {
        if (_form != null)
        {
            _form.SetActive(false);
        }

        if (_state == SkinState.Affordable || _state == SkinState.Unaffordable)
        {
            _tag.text = price;
        }
    }

    public void TurnOffActivity()
    {
        if (_form != null)
        {
            _form.SetActive(false);
        }

    }
}

public enum SkinState
{
    Affordable,
    Unaffordable,
    Selected,
    Unselected
}
