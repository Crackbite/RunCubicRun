using UnityEngine;

namespace AllIn1VfxToolkit
{
    public class All1VfxRandomTimeSeed : MonoBehaviour
    {
        [SerializeField] private float minSeedValue = 0;
        [SerializeField] private float maxSeedValue = 100f;

        private readonly string PropertieName = "_TimingSeed";

        private void Start()
        {
            MaterialPropertyBlock properties = new MaterialPropertyBlock();
            properties.SetFloat(PropertieName, Random.Range(minSeedValue, maxSeedValue));
            GetComponent<Renderer>().SetPropertyBlock(properties);
        }
    }
}