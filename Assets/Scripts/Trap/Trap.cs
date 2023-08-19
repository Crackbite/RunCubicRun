using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Trap : MonoBehaviour
{
    [SerializeField] private TrapType _type;
    [SerializeField] private List<GameObject> _splitBodys;
    [SerializeField] private List<GameObject> _wholeBodys;
    [SerializeField] private List<ParticleSystem> _crushEffects;
    [SerializeField] protected ParticleSystem HitEffect;
    [SerializeField] protected Animator Animator;
    [SerializeField] protected float MinSpeed = .6f;
    [SerializeField] protected float MaxSpeed = 1.2f;

    private bool _isCubicCollided;
    private List<Rigidbody> _piecesRigidbody = new List<Rigidbody>();

    protected readonly int SpeedHash = Animator.StringToHash("Speed");
    protected Collider Collider;

    public bool IsSideCollision { get; private set; }
    public TrapType Type => _type;

    private void OnEnable()
    {
        SetSpeed();
    }

    private void Start()
    {
        foreach (GameObject splitBody in _splitBodys)
        {
            Rigidbody[] rigidbodys = splitBody.GetComponentsInChildren<Rigidbody>();

            foreach (Rigidbody pieceRigidbody in rigidbodys)
            {
                _piecesRigidbody.Add(pieceRigidbody);
            }
        }

        Collider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        const float Threshold = 0.001f;

        if (collision.TryGetComponent(out Cubic cubic) && cubic.IsSawing == false)
        {
            _isCubicCollided = true;
            Vector3 cubicPosition = cubic.transform.position;
            Vector3 contactPoint = Collider.ClosestPoint(cubicPosition);

            if (cubic.CanDestroy)
            {
                cubic.SoundSystem.Play(SoundEvent.Crush);
                Break();
                return;
            }

            IsSideCollision = Mathf.Abs(cubicPosition.z - contactPoint.z) > Threshold ||
                transform.position.x < cubicPosition.x;

            float trapHeight = Collider.bounds.max.y;
            cubic.HitTrap(this, contactPoint, trapHeight);
            CompleteCollision(contactPoint, cubic);
        }
        else if (collision.TryGetComponent(out ColorBlock block) && block.CanFollow == false)
        {
            if (_isCubicCollided == false)
            {
                Collider.isTrigger = false;
            }
        }
        else if (collision.TryGetComponent(out SplittedPart splittedPart))
        {
            if (splittedPart.transform.parent == null)
            {
                Collider.isTrigger = false;
            }
        }
    }

    protected virtual void CompleteCollision(Vector3 contactPoint, Cubic cubic)
    {
        cubic.SoundSystem.Play(SoundEvent.TrapHit);
        HitEffect.transform.position = contactPoint;
        HitEffect.Play();
        Stop();
        Collider.isTrigger = false;
    }

    protected virtual void SetSpeed()
    {
        float speed = Random.Range(MinSpeed, MaxSpeed);
        Animator.SetFloat(SpeedHash, speed);
    }

    protected void Stop()
    {
        if (Animator != null)
        {
            Animator.enabled = false;
        }
    }

    private void SwitchActivity(List<GameObject> bodys, bool isActive)
    {
        foreach (GameObject body in bodys)
        {
            body.SetActive(isActive);
        }
    }

    private void Break()
    {
        const float PieceLifeTime = 10f;
        const float DragDistance = 2f;
        const float DragDuration = 1f;

        Stop();
        SwitchActivity(_wholeBodys, false);
        SwitchActivity(_splitBodys, true);

        foreach (ParticleSystem crushEffect in _crushEffects)
        {
            crushEffect.Play();
        }

        foreach (Rigidbody piece in _piecesRigidbody)
        {
            piece.isKinematic = false;
            Vector3 randomDirection = Random.onUnitSphere;

            if (randomDirection.z > 0f)
            {
                piece.transform.DOMoveZ(piece.transform.position.z + DragDistance, DragDuration).SetEase(Ease.OutQuad);
            }
            else
            {
                piece.transform.DOMoveZ(piece.transform.position.z - DragDistance, DragDuration).SetEase(Ease.OutQuad);
            }

            Destroy(piece.gameObject, PieceLifeTime);
        }
    }
}