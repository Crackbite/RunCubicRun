using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Spring : MonoBehaviour
{
    [SerializeField] private Abyss _abyss;
    [SerializeField] private ParticleSystem _landingEffect;

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

        if(cubic.IsSawing == false)
        {
             StartCoroutine(ThrowOverAbyss(cubic));
        }
    }

    private IEnumerator ThrowOverAbyss(Cubic cubic)
    {
        float runningTime = 0f;
        float startPositionY = cubic.transform.position.y;
        bool isGround = false;

        while (isGround == false)
        {
            runningTime += Time.fixedDeltaTime;

            Vector3 currentPosition = cubic.transform.position;
            currentPosition.y = startPositionY
                                + ((cubic.JumpForce * runningTime) - ((cubic.JumpAcceleration * Mathf.Pow(runningTime, 2f)) / 2f));

            if (currentPosition.y < startPositionY)
            {
                currentPosition.y = startPositionY;
                _landingEffect.transform.position = new Vector3(currentPosition.x, _landingEffect.transform.position.y, currentPosition.z);
                _landingEffect.Play();
                cubic.SoundSystem.Play(SoundEvent.GroundImpact);
                isGround = true;
            }

            cubic.transform.position = currentPosition;
            yield return new WaitForFixedUpdate();
        }
    }
}