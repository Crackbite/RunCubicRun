using UnityEngine;
using UnityEngine.Events;

public class Cubic : MonoBehaviour
{
    [SerializeField] private bool _canDestroy;

    public event UnityAction Hit;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Trap>(out Trap trap))
        {
            if (_canDestroy)
            {               
                trap.Break();
            }
            else
            {
                trap.Stop();
                Hit.Invoke();
            }
        }
    }
}
