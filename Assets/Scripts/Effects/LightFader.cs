using UnityEngine;

namespace EpicToonFX
{
    public class LightFader : MonoBehaviour
    {
        [SerializeField] private float _lifeTime = 0.2f;
        [SerializeField] private bool _killAfterLife = true;

        private Light _light;
        private float _initialIntensity;

        private void Start()
        {
            if (TryGetComponent<Light>(out Light light))
            {
                _light = light;
                _light.enabled = true;
                _initialIntensity = _light.intensity;
            }
        }

        private void Update()
        {
            if (_light == null)
            {
                return;
            }

            _light.intensity -= _initialIntensity * (Time.deltaTime / _lifeTime);

            if (_killAfterLife && _light.intensity <= 0)
            {
                Destroy(_light);
            }
        }
    }
}