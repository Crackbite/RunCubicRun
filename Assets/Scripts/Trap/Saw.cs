using System;
using System.Collections;
using UnityEngine;

public abstract class Saw : Trap
{
    [SerializeField] private MeshRenderer _meshRenderer;

    private Bounds _bounds => _meshRenderer.bounds;

    public event Action<Cubic> CameOut;

    protected IEnumerator CheckIntersectionWithCubic(Cubic cubic)
    {
        bool canStopChecking = false;

        while (canStopChecking == false)
        {
            canStopChecking = true;

            foreach (MeshRenderer meshRenderer in cubic.MeshRenderers)
            {
                if (_bounds.Intersects(meshRenderer.bounds))
                {
                    canStopChecking = false;
                }
            }

            yield return null;
        }

        cubic.SoundSystem.Stop(SoundEvent.Sawing);
        CameOut?.Invoke(cubic);
    }
}