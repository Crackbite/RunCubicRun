using System;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public event Action<Checkpoint> CubicPassed;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent(out Cubic cubic) == false || cubic.IsSawing)
        {
            return;
        }

        CubicPassed?.Invoke(this);
    }
}
