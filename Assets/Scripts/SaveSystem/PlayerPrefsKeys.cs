using System.Collections.Generic;

public static class PlayerPrefsKeys 
{
    public const string ScoreKey = nameof(ScoreKey);
    public const string LeaderboardScoreKey = nameof(LeaderboardScoreKey);
    public const string LevelKey = nameof(LevelKey);
    public const string TrainingStageKey = nameof(TrainingStageKey);
    public const string ActiveKey = nameof(ActiveKey);
    public const string BoughtKey = nameof(BoughtKey);
    public const string RestartKey = nameof(RestartKey);
    public const string AuthKey = nameof(AuthKey);
    public const string MusicToggleKey = nameof(MusicToggleKey);
    public const string SoundToggleKey = nameof(SoundToggleKey);

    static PlayerPrefsKeys()
    {
        Keys = new List<string>
    {
        ScoreKey,
        LeaderboardScoreKey,
        LevelKey,
        TrainingStageKey,
        ActiveKey,
        BoughtKey,
        RestartKey,
        AuthKey,
    };
    }

    public static IReadOnlyCollection<string> Keys { get; }

}
