using UnityEngine;

public class Chunk : MonoBehaviour
{
    [SerializeField] private int _difficulty;
    [SerializeField] private bool _canRotate;

    public bool CanRotate => _canRotate;
    public int Difficulty => _difficulty;
}