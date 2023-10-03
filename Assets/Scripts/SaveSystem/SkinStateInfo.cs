using UnityEngine;

[System.Serializable]
public class SkinStateInfo
{
    [SerializeField] private string _id;
    [SerializeField] private bool _isActive;
    [SerializeField] private bool _isBought;

    public string ID => _id;
    public bool IsActive => _isActive;
    public bool IsBought => _isBought;

    public void Init(bool isActive, bool isBought)
    {
        _isActive = isActive;
        _isBought = isBought;
    }

    public void AssignDefoult()
    {
        _isActive = true;
        _isBought = true;
    }

    public void SetIsActive(bool isActive)
    {
        _isActive = isActive;
    }

    public void SetIsBought(bool isBought)
    {
        _isBought = isBought;
    }
}
