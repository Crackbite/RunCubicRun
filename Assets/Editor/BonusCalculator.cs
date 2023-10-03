using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

public class BonusCalculator
{
    private readonly ReadOnlyDictionary<Type, int> _bonusDifficulties = new ReadOnlyDictionary<Type, int>(
        new Dictionary<Type, int>
        {
            { typeof(Multiplier), 0 },
            { typeof(Colorizer), -2 },
            { typeof(Crusher), -2 },
            { typeof(Hider), -1 },
            { typeof(Accelerator), 2 },
            { typeof(Exploder), 1 },
            { typeof(Blinder), 0 },
            { typeof(Reverser), 2 }
        });

    public int NegativeBonusesCount { get; private set; }
    public int NegativeBonusesDifficulty { get; private set; }
    public int PositiveBonusesCount { get; private set; }
    public int PositiveBonusesDifficulty { get; private set; }

    public int TotalBonusesCount => PositiveBonusesCount + NegativeBonusesDifficulty;
    public int TotalBonusesDifficulty { get; private set; }

    public void Reset()
    {
        NegativeBonusesCount = 0;
        NegativeBonusesDifficulty = 0;
        PositiveBonusesCount = 0;
        PositiveBonusesDifficulty = 0;
        TotalBonusesDifficulty = 0;
    }

    public void ProcessBonus(Bonus bonus)
    {
        if (_bonusDifficulties.TryGetValue(bonus.GetType(), out int difficulty) == false)
        {
            return;
        }

        if (bonus.Info.IsPositive)
        {
            PositiveBonusesCount++;
            PositiveBonusesDifficulty += difficulty;
        }
        else
        {
            NegativeBonusesCount++;
            NegativeBonusesDifficulty += difficulty;
        }

        TotalBonusesDifficulty += difficulty;
    }

    public override string ToString()
    {
        var sb = new StringBuilder(3);

        sb.AppendLine($"Bonus: {TotalBonusesCount}");
        sb.AppendLine($"   Positive: {PositiveBonusesCount} ({PositiveBonusesDifficulty:+#;-#;0})");
        sb.Append($"   Negative: {NegativeBonusesCount} ({NegativeBonusesDifficulty:+#;-#;0})");

        return sb.ToString();
    }
}