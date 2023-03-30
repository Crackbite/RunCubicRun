public sealed class Score
{
    public Score(float current, float change)
    {
        Current = current;
        Change = change;
    }

    public float Change { get; }

    public float Current { get; }
}