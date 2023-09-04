using UnityEngine;

[System.Serializable]
public class ChunkData
{
    [SerializeField] private string _name;
    [SerializeField] private Quaternion _rotation;

    public ChunkData(string name, Quaternion rotation)
    {
        _name = name;
        _rotation = rotation;
    }

    public string Name => _name;
    public Quaternion Rotation => _rotation;
}