using UnityEngine;
using UnityEngine.Events;

public class Trap : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    public event UnityAction Destroying;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Cubic>(out Cubic cubic))
        {
            if (cubic.CanDestroy)
            {
                Destroying.Invoke();
                _animator.enabled = false;
            }

        }
    }
}
