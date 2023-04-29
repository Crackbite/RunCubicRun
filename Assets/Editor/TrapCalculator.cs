using System.Collections.Generic;
using System.Collections.ObjectModel;

public class TrapCalculator
{
    private readonly ReadOnlyDictionary<TrapType, int> _trapDifficulties = new ReadOnlyDictionary<TrapType, int>(
        new Dictionary<TrapType, int>
        {
            { TrapType.Crossbar, 1 },
            { TrapType.Door, 1 },
            { TrapType.MovingGrinder, 2 },
            { TrapType.PatrolSaw, 2 },
            { TrapType.PushingPillar, 2 },
            { TrapType.Rotator, 2 },
            { TrapType.SpinningSaw, 1 },
            { TrapType.StaticGrinder, 1 },
            { TrapType.StaticSaw, 1 },
            { TrapType.SpinningStick, 2 }
        });

    public int TotalTrapsDifficulty { get; private set; }
    public int TrapsCount { get; private set; }

    public void Reset()
    {
        TotalTrapsDifficulty = 0;
        TrapsCount = 0;
    }

    public void ProcessTrapByType(TrapType trapType)
    {
        if (_trapDifficulties.TryGetValue(trapType, out int difficulty) == false)
        {
            return;
        }

        TrapsCount++;
        TotalTrapsDifficulty += difficulty;
    }

    public override string ToString()
    {
        return $"Traps: {TrapsCount} ({TotalTrapsDifficulty:+#;-#;0})";
    }
}