public class DistanceCalculator
{
    public float AverageDistance => DistancesCount < 1 ? 0 : TotalDistance / DistancesCount;
    public int DistancesCount { get; private set; }
    public float TotalDistance { get; private set; }
    public int TotalDistancesDifficulty { get; private set; }

    public void Reset()
    {
        DistancesCount = 0;
        TotalDistance = 0f;
        TotalDistancesDifficulty = 0;
    }

    public void ProcessDistance(float distance)
    {
        if (distance <= 5.5f)
        {
            TotalDistancesDifficulty += 2;
        }

        if (distance >= 5.6 && distance <= 7)
        {
            TotalDistancesDifficulty += 1;
        }

        DistancesCount++;
        TotalDistance += distance;
    }

    public override string ToString()
    {
        return $"Distance: {AverageDistance:F2} ({TotalDistancesDifficulty:+#;-#;0})";
    }
}