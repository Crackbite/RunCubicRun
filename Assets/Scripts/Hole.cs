using UnityEngine;

public class Hole : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent(out Cubic cubic) == false)
        {
            return;
        }

        if (cubic.TryGetComponent(out Rigidbody collisionRigidbody))
        {
            collisionRigidbody.isKinematic = false;
        }
    }
}
