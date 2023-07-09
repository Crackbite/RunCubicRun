using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Confetti : MonoBehaviour
{
    [SerializeField] private List<ParticleSystem> _particles;
    [SerializeField] private CubicMovement _cubicMovement;
    [SerializeField] private float _delayBetweenBurst;

    private WaitForSeconds _waitForSeconds;

    private void OnEnable()
    {
        _cubicMovement.CubicLeftPress += OnCubicLeftPress;
    }

    private void Awake()
    {
        _waitForSeconds = new WaitForSeconds(_delayBetweenBurst);
    }

    private void OnDisable()
    {
        _cubicMovement.CubicLeftPress -= OnCubicLeftPress;
    }

    private void OnCubicLeftPress()
    {
        StartCoroutine(Launch());
    }

    private IEnumerator Launch()
    {
        const int AmountPerBurst = 2;

        while (_particles.Count > 0)
        {
            for (int i = 0; i < AmountPerBurst; i++)
            {
                if (_particles[0] != null)
                {
                    _particles[0].Play();
                    _particles.Remove(_particles[0]);
                }
            }

            yield return _waitForSeconds;
        }
    }
}
