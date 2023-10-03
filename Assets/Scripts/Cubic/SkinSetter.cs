using System.Collections.Generic;
using UnityEngine;

public class SkinSetter : MonoBehaviour
{
    [SerializeField] private List<MeshRenderer> _cubicPartRenderers;
    [SerializeField] private List<MeshFilter> _cubicPartMeshFilters;
    [SerializeField] private SkinsRestorer _skinsRestorer;


    private void OnEnable()
    {
        foreach (Skin skin in _skinsRestorer.Skins)
        {
            skin.ActivityChanged += OnSkinActivityChanged;
        }
    }

    private void OnDisable()
    {
        foreach (Skin skin in _skinsRestorer.Skins)
        {
            skin.ActivityChanged -= OnSkinActivityChanged;
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

    private void OnSkinActivityChanged(Skin skin)
    {
        if (skin.IsActive)
        {
            Set(skin);
        }
    }
}
