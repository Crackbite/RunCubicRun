using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TrainingStageInfo", menuName = "LevelData/TrainingStageInfo", order = 51)]
public class TrainingStageInfo : ScriptableObject
{
    [SerializeField] private int _number;
    [SerializeField] private List<Chunk> _chunks;
    [SerializeField] private List<GameObject> _trainingPhrases;

    public int Number => _number;
    public IReadOnlyList<Chunk> Chunks => _chunks;
    public IReadOnlyList<GameObject> TrainingPhrases => _trainingPhrases;
}
