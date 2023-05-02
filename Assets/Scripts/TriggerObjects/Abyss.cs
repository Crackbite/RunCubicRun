using UnityEngine;

public class Abyss : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent(out Cubic cubic) == false)
        {
            return;
        }

        if (cubic.TryGetComponent(out Rigidbody cubicRigidbody))
        {
            cubicRigidbody.isKinematic = false;
        }
    }
}
