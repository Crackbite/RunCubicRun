using UnityEngine;
using UnityEngine.Events;

public class Trap : MonoBehaviour
{
    public event UnityAction Destroying;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Cubic>(out Cubic cubic))
        {
            if(cubic.CanDestroy)
                Destroying.Invoke();
        }
    }
}
