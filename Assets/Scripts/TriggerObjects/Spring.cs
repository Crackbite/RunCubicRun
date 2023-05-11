using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Spring : MonoBehaviour
{
    [SerializeField] private Abyss _abyss;

    private readonly int _tossHash = Animator.StringToHash("Toss");

    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();

        if (transform.position.x > _abyss.transform.position.x)
        {
            Vector3 localPosition = transform.localPosition;
            transform.localPosition = new Vector3(-localPosition.x, localPosition.y, localPosition.z);

            var halfTurnAngle = new Vector3(0f, 180f, 0f);
            transform.Rotate(halfTurnAngle);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent(out Cubic cubic))
        {
            Toss(cubic);
        }
    }

    public void Toss(Cubic cubic)
    {
        _animator.SetTrigger(_tossHash);
        StartCoroutine(ThrowOverAbyss(cubic.transform, cubic.JumpForce, cubic.JumpAcceleration));
    }

    private IEnumerator ThrowOverAbyss(Transform flyingObject, float throwForce, float acceleration)
    {
        float runningTime = 0f;
        float startPositionY = flyingObject.position.y;
        bool isGround = false;

        while (isGround == false)
        {
            runningTime += Time.deltaTime;

            Vector3 currentPosition = flyingObject.position;
            currentPosition.y = startPositionY
                                + ((throwForce * runningTime) - ((acceleration * Mathf.Pow(runningTime, 2f)) / 2f));

            if (currentPosition.y < startPositionY)
            {
                currentPosition.y = startPositionY;
                isGround = true;
            }

            flyingObject.transform.position = currentPosition;
            yield return null;
        }
    }
}