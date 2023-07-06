using UnityEngine;
using System.Collections;

namespace EpicToonFX
{
    public class LightFader : MonoBehaviour
    {
        [SerializeField] private float _lifeTime = 0.2f;
        [SerializeField] private bool _killAfterLife = true;

        private Light _light;
        private float _initIntensity;

        private void Start()
        {
            if (TryGetComponent<Light>(out Light light))
            {
                _light = light;
                _initIntensity = _light.intensity;
            }
            else
                print("No light object found on " + gameObject.name);
        }

        private void Update()
        {
            if (_light != null)
            {
                _light.intensity -= _initIntensity * (Time.deltaTime / _lifeTime);

                if (_killAfterLife && _light.intensity <= 0)
                {
					Destroy(_light);
                }
            }
        }
    }
}