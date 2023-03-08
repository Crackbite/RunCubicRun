using System;
using UnityEngine;

public class Cubic : MonoBehaviour
{
    private bool _steppedOnStand;

    public event Action<PressStand> SteppedOnStand;

    private void OnTriggerEnter(Collider collision)
    {
        if (_steppedOnStand || collision.TryGetComponent(out PressStand pressStand) == false)
        {
            return;
        }

        _steppedOnStand = true;
        SteppedOnStand?.Invoke(pressStand);
    }
}