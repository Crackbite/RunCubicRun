using System.Collections.Generic;
using UnityEngine;

public class SkinSetter : MonoBehaviour
{
    [SerializeField] private List<Skin> _skins;
    [SerializeField] private List<MeshRenderer> _cubicPartRenderers;
    [SerializeField] private List<MeshFilter> _cubicPartMeshFilters;


    private void OnEnable()
    {
        foreach (Skin skin in _skins)
        {
            if (skin.IsActive)
            {
                Set(skin);
            }

            skin.Activated += OnSkinActivated;
        }
    }

    private void OnDisable()
    {
        foreach (Skin skin in _skins)
        {
            skin.Activated -= OnSkinActivated;
        }
    }

    private void Set(Skin skin)
    {
        int materialCount = skin.Materials.Count;
        Material[] materials = new Material[materialCount];

        for (int i = 0; i < materialCount; i++)
        {
            materials[i] = skin.Materials[i];
        }

        foreach (MeshFilter meshFilter in _cubicPartMeshFilters)
        {
            meshFilter.mesh = skin.HalfSkinMesh;
        }

        foreach (MeshRenderer renderer in _cubicPartRenderers)
        {
            renderer.sharedMaterials = materials;
        }
    }

    private void OnSkinActivated(Skin skin)
    {
        Set(skin);
    }
}
