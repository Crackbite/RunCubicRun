public class BonusItem
{
    public BonusItem(Bonus bonus, BonusIcon bonusIcon, BonusTimer bonusTimer)
    {
        Bonus = bonus;
        BonusIcon = bonusIcon;
        BonusTimer = bonusTimer;
    }

    public Bonus Bonus { get; }
    public BonusIcon BonusIcon { get; }
    public BonusTimer BonusTimer { get; }
}