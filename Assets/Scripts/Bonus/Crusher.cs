using System.Collections.Generic;
using UnityEngine;

public class Crusher : Bonus
{
    [SerializeField] private Transform _trapsContainer;

    private HashSet<Trap> _modifiedTraps;

    public override void Apply()
    {
        Trap[] traps = _trapsContainer.GetComponentsInChildren<Trap>();
        _modifiedTraps = new HashSet<Trap>(traps.Length);

        foreach (Trap trap in traps)
        {
            trap.CanBreak = true;
            _modifiedTraps.Add(trap);
        }
    }

    public override void Cancel()
    {
        foreach (Trap modifiedTrap in _modifiedTraps)
        {
            modifiedTrap.CanBreak = false;
        }
    }
}