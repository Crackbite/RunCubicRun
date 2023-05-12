using UnityEngine;

public class ChunkData
{
    public ChunkData(string name, Quaternion rotation)
    {
        Name = name;
        Rotation = rotation;
    }

    public string Name { get; }
    public Quaternion Rotation { get; }
}