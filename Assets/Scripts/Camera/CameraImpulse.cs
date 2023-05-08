using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineImpulseSource))]
public class CameraImpulse : MonoBehaviour
{
    private CinemachineImpulseSource _cinemachineImpulse;
    private bool _isActivated;

    private void Start()
    {
        _cinemachineImpulse = GetComponent<CinemachineImpulseSource>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (_isActivated || collision.TryGetComponent(out ColorBlock _) == false)
        {
            return;
        }

        _isActivated = true;
        _cinemachineImpulse.GenerateImpulse();
    }
}