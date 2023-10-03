using UnityEngine;

namespace SciFiArsenal
{
    public class ParticlesRotator : MonoBehaviour
    {
        [SerializeField] private Vector3 _rotateVector = Vector3.zero;

        private SpaceEnum _rotateSpace;

        private enum SpaceEnum { Local, World };

        private void Update()
        {
            if (_rotateSpace == SpaceEnum.Local)
            {
                transform.Rotate(_rotateVector * Time.deltaTime);
            }
            if (_rotateSpace == SpaceEnum.World)
            {
                transform.Rotate(_rotateVector * Time.deltaTime, Space.World);
            }
        }
    }
}