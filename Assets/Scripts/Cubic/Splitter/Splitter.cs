using UnityEngine;

public abstract class Splitter : MonoBehaviour
{
    [SerializeField] protected Transform FirstPart;
    [SerializeField] protected Transform SecondPart;

    public abstract void Split();

    public abstract void SplitOnePart(Transform part);
}