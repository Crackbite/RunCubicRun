using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Spring : MonoBehaviour
{
    [SerializeField] private float _throwForce;
    [SerializeField] private AnimationCurve _throwCurve;

    private Animator _animator;
    private const string _animationName = "Toss";

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Cubic>(out Cubic cubic))
        {
            _animator.Play(_animationName);
            StartCoroutine(Toss(cubic.transform));
        }
    }

    private IEnumerator Toss(Transform cubic)
    {
        float runningTime = 0;
        float duration;
        Vector3 startPosition = cubic.position;
        Vector3 currentPosition;

        duration = _throwCurve.keys[_throwCurve.length - 1].time;

        while (runningTime <= duration)
        {
            currentPosition = cubic.position;
            currentPosition.y = startPosition.y + _throwForce * _throwCurve.Evaluate(runningTime);
            cubic.transform.position = currentPosition;
            runningTime += Time.deltaTime;
            yield return null;
        }
    }
}
