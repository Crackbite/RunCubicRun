using UnityEngine;

public class Saw : Trap
{
    [SerializeField] private bool _isVertical;

    public bool IsVertical => _isVertical;
}
