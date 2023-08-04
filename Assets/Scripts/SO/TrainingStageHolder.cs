using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TrainingStageHolder", menuName = "LevelData/TrainingStageHolder", order = 51)]
public class TrainingStageHolder : ScriptableObject
{
    [SerializeField] private List<TrainingStageInfo> _stageInfoList;

    public int StageAmount { get; private set; }

    private void OnValidate()
    {
        StageAmount = _stageInfoList.Count;
    }

    public bool TryGetStageInfo(int stageNumber, out TrainingStageInfo currentStageInfo)
    {
        foreach (TrainingStageInfo stageInfo in _stageInfoList)
        {
            if (stageInfo.Number == stageNumber)
            {
                currentStageInfo = stageInfo;
                return true;
            }
        }

        currentStageInfo = null;
        return false;
    }
}
