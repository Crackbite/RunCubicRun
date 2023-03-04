using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ColorStacker : MonoBehaviour
{
    [SerializeField] private float _gap = .02f;
    [SerializeField] private float _speed = .1f;
    [SerializeField] private Transform _blocksContainer;

    private float _currentYPosition;
    private Coroutine _takeBlockRoutine;

    private void Start()
    {
        _currentYPosition += _gap;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent<ColorBlock>(out ColorBlock colorBlock))
        {
            colorBlock.transform.SetParent(_blocksContainer);

            if (_takeBlockRoutine != null)
            {
                StopCoroutine(_takeBlockRoutine);
                _takeBlockRoutine = null;
            }

           _takeBlockRoutine = StartCoroutine(TakeBlock(colorBlock.transform));
        }
    }

    private IEnumerator TakeBlock(Transform block)
    {
        yield return block.DOLocalMove(Vector3.zero, _speed).WaitForCompletion();

        block.localPosition = new Vector3(0f, _currentYPosition, 0f);
        _currentYPosition += block.localScale.y + _gap;
    }
}
