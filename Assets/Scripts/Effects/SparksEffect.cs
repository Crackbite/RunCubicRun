using DG.Tweening;
using System;
using UnityEngine;

public class SparksEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystem _sparks;
    [SerializeField] private Cubic _cubic;
    [SerializeField] private float _duration;
    [SerializeField] private Vector3[] _verticalPath;
    [SerializeField] private Vector3[] _horizontalRightPath;
    [SerializeField] private Vector3[] _horizontalLeftPath;

    private Vector3[] _path = new Vector3[0];
    private bool _isSawCameOut;

    private void OnEnable()
    {
        _cubic.SawingStarted += OnSawingStarted;

    }

    private void OnDisable()
    {
        _cubic.SawingStarted -= OnSawingStarted;
    }

    private void OnSawingStarted()
    {
        _cubic.CollisionSaw.CameOut += OnSawCameOut;
        ChoosePath(_cubic.CollisionSaw);
        _sparks.transform.localPosition = _path[0];
        _sparks.Play();

        _sparks.transform.DOLocalPath(_path, _duration, PathType.Linear)
            .SetEase(Ease.Linear)
            .OnUpdate(() =>
            {
                if (_isSawCameOut == true)
                {
                    _sparks.Stop();
                }
            })
            .OnComplete(() => 
            {
                _sparks.Stop();
                _cubic.CollisionSaw.CameOut -= OnSawCameOut;
            });

    }

    private void OnSawCameOut()
    {
        _isSawCameOut = true;
    }

    private void ChoosePath(Saw saw)
    {
        if (saw is VerticalSaw)
        {
            _path = _verticalPath;
        }
        else if (saw.TryGetComponent(out HorizontalSaw horizontalSaw))
        {
            if (horizontalSaw.Side == SawSide.Left)
            {
                _path = _horizontalLeftPath;
            }
            else
            {
                _path = _horizontalRightPath;
            }

            if (saw.transform.position.x > _cubic.transform.position.x)
            {
                Array.Reverse(_path);
            }
        }

    }
}
