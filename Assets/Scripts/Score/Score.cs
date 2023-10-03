public sealed class Score
{
    public Score(float current, float change, ScoreChangeInitiator initiator)
    {
        Current = current;
        Change = change;
        Initiator = initiator;
    }

    public float Change { get; }
    public float Current { get; }
    public ScoreChangeInitiator Initiator { get; }
}