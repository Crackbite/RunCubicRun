using System;
using UnityEngine;

public class TimeInvoker : MonoBehaviour
{
    private static TimeInvoker instance;

    private float _oneSecTimer;

    public event Action OnOneSyncedSecondTickedEvent;

    public static TimeInvoker Instance
    {
        get
        {
            if (instance == null)
            {
                var go = new GameObject("[TIME INVOKER]");
                instance = go.AddComponent<TimeInvoker>();
                DontDestroyOnLoad(go);
            }

            return instance;
        }
    }

    private void Update()
    {
        float deltaTimer = Time.deltaTime;

        _oneSecTimer += deltaTimer;

        if (_oneSecTimer >= 1f)
        {
            _oneSecTimer -= 1f;

            OnOneSyncedSecondTickedEvent?.Invoke();
        }
    }
}