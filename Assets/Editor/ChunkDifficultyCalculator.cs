using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class ChunkDifficultyCalculator : EditorWindow
{
    private const int AbyssBaseDifficulty = 1;
    private const int PortalBaseDifficulty = 1;

    private BonusCalculator _bonusCalculator;
    private int _colorBlockCount;
    private DistanceCalculator _distanceCalculator;
    private bool _hasAbyss;
    private string _lastUpdateTime = "-";
    private string _objectName = "-";
    private int _portalsCount;
    private int _portalsDifficulty;
    private int _totalDifficulty;
    private TrapCalculator _trapCalculator;

    private void OnEnable()
    {
        _bonusCalculator = new BonusCalculator();
        _distanceCalculator = new DistanceCalculator();
        _trapCalculator = new TrapCalculator();
    }

    private void OnGUI()
    {
        ShowCalculationDetails();

        if (GUILayout.Button("Calculate Difficulty"))
        {
            CalculateDifficulty();
        }
    }

    [MenuItem("Window/Chunk Difficulty Calculator")]
    public static void ShowWindow()
    {
        GetWindow(typeof(ChunkDifficultyCalculator), false, "Chunk Difficulty Calculator");
    }

    private void CalculateAbyssesDifficulty(GameObject mainObject)
    {
        _hasAbyss = mainObject.GetComponentInChildren<Spring>() != null;

        if (_hasAbyss)
        {
            _totalDifficulty += AbyssBaseDifficulty;
        }
    }

    private void CalculateBonusesDifficulty(GameObject mainObject)
    {
        _bonusCalculator.Reset();
        Bonus[] bonuses = mainObject.GetComponentsInChildren<Bonus>();

        foreach (Bonus bonus in bonuses)
        {
            _bonusCalculator.ProcessBonus(bonus);
        }

        _totalDifficulty += _bonusCalculator.TotalBonusesDifficulty;
    }

    private void CalculateDifficulty()
    {
        GameObject activeGameObject = Selection.activeGameObject;

        if (activeGameObject == null || activeGameObject.GetComponentInChildren<Chunk>() == null)
        {
            return;
        }

        _totalDifficulty = 0;
        _objectName = activeGameObject.name;

        _colorBlockCount = activeGameObject.GetComponentsInChildren<ColorBlock>().Length;

        CalculateBonusesDifficulty(activeGameObject);
        CalculateTrapsDifficulty(activeGameObject);
        CalculateAbyssesDifficulty(activeGameObject);
        CalculatePortalsDifficulty(activeGameObject);
        CalculateDistancesDifficulty(activeGameObject);

        _lastUpdateTime = DateTime.Now.ToString("T");
    }

    private void CalculateDistancesDifficulty(GameObject mainObject)
    {
        _distanceCalculator.Reset();
        Animator[] animators = mainObject.GetComponentsInChildren<Animator>();

        Vector3 startPos = GetChunkStartPosition(mainObject);
        animators = animators.OrderByDescending(animator => Vector3.Distance(animator.transform.position, startPos))
            .ToArray();

        for (int i = 0; i < animators.Length - 1; i++)
        {
            float distance = Vector3.Distance(animators[i].transform.position, animators[i + 1].transform.position);
            _distanceCalculator.ProcessDistance(distance);
        }

        _totalDifficulty += _distanceCalculator.TotalDistancesDifficulty;
    }

    private void CalculatePortalsDifficulty(GameObject mainObject)
    {
        _portalsCount = mainObject.GetComponentsInChildren<Portal>().Length;
        _portalsDifficulty = _portalsCount * PortalBaseDifficulty;

        _totalDifficulty += _portalsDifficulty;
    }

    private void CalculateTrapsDifficulty(GameObject mainObject)
    {
        _trapCalculator.Reset();
        Animator[] animators = mainObject.GetComponentsInChildren<Animator>();

        foreach (Animator animator in animators)
        {
            var trap = animator.GetComponentInChildren<Trap>();

            if (trap != null)
            {
                _trapCalculator.ProcessTrapByType(trap.Type);
            }
        }

        StackReducer[] stackReducers = mainObject.GetComponentsInChildren<StackReducer>();

        foreach (StackReducer stackReducer in stackReducers)
        {
            var crossbar = stackReducer.GetComponentInChildren<Crossbar>(true);

            if (crossbar != null)
            {
                _trapCalculator.ProcessTrapByType(crossbar.Type);
            }
        }

        _totalDifficulty += _trapCalculator.TotalTrapsDifficulty;
    }

    private Vector3 GetChunkStartPosition(GameObject mainObject)
    {
        var meshRenderer = mainObject.GetComponentInChildren<Road>().GetComponent<MeshRenderer>();
        float maxX = meshRenderer.bounds.max.x;

        return new Vector3(maxX, 0f, 0f);
    }

    private void ShowCalculationDetails()
    {
        GUILayout.Label($"Object: {_objectName}");
        GUILayout.Label($"Update: {_lastUpdateTime}");
        GUILayout.Space(10);
        GUILayout.Label($"ColorBlock: {_colorBlockCount}");
        GUILayout.Label(_bonusCalculator.ToString());
        GUILayout.Label(_trapCalculator.ToString());
        GUILayout.Label($"Abyss: {_hasAbyss} ({(_hasAbyss ? AbyssBaseDifficulty : 0):+#;-#;0})");
        GUILayout.Label($"Portals: {_portalsCount} ({(_portalsCount > 0 ? _portalsDifficulty : 0):+#;-#;0})");
        GUILayout.Label(_distanceCalculator.ToString());
        GUILayout.Label($"Difficulty: {_totalDifficulty}");
    }
}