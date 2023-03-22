using UnityEngine;

public class Hole : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Cubic cubic) == false)
        {
            return;
        }

        if (cubic.TryGetComponent(out Rigidbody rigidbody))
        {
            rigidbody.isKinematic = false;
        }
    }
}
