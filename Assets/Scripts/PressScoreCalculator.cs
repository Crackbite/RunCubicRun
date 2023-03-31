using System.Collections.Generic;
using UnityEngine;

public class PressScoreCalculator : MonoBehaviour
{
    [Range(.1f, 100f)] [SerializeField] private float _minScore = .2f;
    [Range(1f, 10000f)] [SerializeField] private float _maxScore = 15f;
    [Range(1, 100)] [SerializeField] private int _increaseStep = 5;
    [Range(.1f, 100f)] [SerializeField] private float _increaseAmount = .4f;
    [SerializeField] private Cubic _cubic;
    [SerializeField] private BlockStack _blockStack;

    private Dictionary<ColorBlock, float> _colorBlocksScore;

    private void OnEnable()
    {
        _cubic.SteppedOnStand += OnCubicSteppedOnStand;
    }

    private void OnDisable()
    {
        _cubic.SteppedOnStand -= OnCubicSteppedOnStand;
    }

    public float GetScore(ColorBlock colorBlock)
    {
        const float DefaultValue = 0f;

        if (_colorBlocksScore == null)
        {
            return DefaultValue;
        }

        return _colorBlocksScore.ContainsKey(colorBlock) ? _colorBlocksScore[colorBlock] : DefaultValue;
    }

    private void OnCubicSteppedOnStand(PressStand pressStand)
    {
        _colorBlocksScore = new Dictionary<ColorBlock, float>(_blockStack.Blocks.Count);

        int increaseStepCounter = 0;
        float currentBlockScore = 0f;

        for (int i = 0; i < _blockStack.Blocks.Count; i++)
        {
            ColorBlock colorBlock = _blockStack.Blocks[i];
            currentBlockScore += _minScore + (_increaseAmount * increaseStepCounter);

            if (currentBlockScore > _maxScore)
            {
                currentBlockScore = _maxScore;
            }
            else if ((i + 1) % _increaseStep == 0)
            {
                increaseStepCounter++;
            }

            _colorBlocksScore.Add(colorBlock, currentBlockScore);
        }
    }
}