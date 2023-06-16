using TMPro;
using UnityEngine;

[System.Serializable]
public class SkinStateForm
{
    [SerializeField] private GameObject _form;
    [SerializeField] private TMP_Text _tag;
    [SerializeField] private SkinState _state;

    public SkinState State => _state;
    public bool IsActive => _form.activeSelf;

    public void Set(string price)
    {
        _form.SetActive(true);

        if (_state == SkinState.Affordable || _state == SkinState.Unaffordable)
        {
            _tag.text = price;
        }
    }

    public void TurnOffActivity()
    {
        _form.SetActive(false);
    }
}

public enum SkinState
{
    Affordable,
    Unaffordable,
    Selected,
    Unselected
}
