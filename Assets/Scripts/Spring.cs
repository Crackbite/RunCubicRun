using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Spring : MonoBehaviour
{
    [SerializeField] private float _throwForce;

    private Animator _animator;
    private const string _triggerName = "Toss";

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void Toss(Cubic cubic)
    {
        _animator.SetTrigger(_triggerName);
        StartCoroutine(ThrowOverHole(cubic.transform, cubic.JumpForce, cubic.JumpAcceleration));
    }

    private IEnumerator ThrowOverHole(Transform flyingObject, float throwForce, float acceleration)
    {
        float runningTime = 0;
        float startPositionY = flyingObject.position.y;
        Vector3 currentPosition;
        bool isGround = false;

        while (isGround == false)
        {
            runningTime += Time.deltaTime;
            currentPosition = flyingObject.position;
            currentPosition.y = startPositionY + throwForce * runningTime - acceleration * Mathf.Pow(runningTime, 2) / 2;

            if (currentPosition.y < startPositionY)
            {
                currentPosition.y = startPositionY;
                isGround = true;
            }

            flyingObject.transform.position = currentPosition;
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Cubic>(out Cubic cubic))
        {
            Toss(cubic);
        }
    }
}
