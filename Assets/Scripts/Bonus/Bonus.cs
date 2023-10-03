using UnityEngine;

public abstract class Bonus : MonoBehaviour
{
    [SerializeField] private BonusInfo _info;

    public BonusInfo Info => _info;

    public abstract void Apply();

    public abstract void Cancel();
}