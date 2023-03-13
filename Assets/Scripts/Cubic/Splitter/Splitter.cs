using UnityEngine;
using DG.Tweening;

public abstract class Splitter : MonoBehaviour
{
    [SerializeField] protected Transform FirstPart;
    [SerializeField] protected Transform SecondPart;

    public abstract void Split();
}
