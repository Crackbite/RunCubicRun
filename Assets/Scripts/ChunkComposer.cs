using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChunkComposer
{
    private List<Chunk> _easyChunks;
    private List<Chunk> _hardChunks;
    private List<Chunk> _mediumChunks;

    public ChunkComposer(Chunk[] chunks)
    {
        SortChunks(chunks);
    }

    public void DebugStatistic()
    {
        Debug.Log($"EasyChunks: {_easyChunks.Count}");
        Debug.Log($"MediumChunks: {_mediumChunks.Count}");
        Debug.Log($"HardChunks: {_hardChunks.Count}");
    }

    public List<Chunk> GetSuitableChunks(int level, int count)
    {
        var chunks = new List<Chunk>(count);

        int totalChunksCount = _easyChunks.Count + _hardChunks.Count + _mediumChunks.Count;
        count = Mathf.Min(totalChunksCount, count);

        while (chunks.Count != count)
        {
            int generalChance = Random.Range(0, 100);
            Chunk selectedChunk = null;

            try
            {
                if (CanSelectEasyChunk(level, generalChance))
                {
                    selectedChunk = GetRandomChunk(_easyChunks);
                }
                else if (CanSelectMediumChunk(level, generalChance))
                {
                    selectedChunk = GetRandomChunk(_mediumChunks);
                }
                else if (CanSelectHardChunk(level, generalChance))
                {
                    selectedChunk = GetRandomChunk(_hardChunks);
                }
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                return chunks;
            }

            if (selectedChunk != null)
            {
                chunks.Add(selectedChunk);
            }
        }

        return chunks;
    }

    private bool CanSelectEasyChunk(int level, int generalChance)
    {
        const float Coefficient = 4f;

        float chance = 100 - ((level - 1) * Coefficient);
        chance = Mathf.Clamp(chance, 0, 100);

        return chance > generalChance;
    }

    private bool CanSelectHardChunk(int level, int generalChance)
    {
        const float Coefficient = 1.5f;

        float chance = (level - 1) * Coefficient;
        chance = Mathf.Clamp(chance, 0, 100);

        return chance > generalChance;
    }

    private bool CanSelectMediumChunk(int level, int generalChance)
    {
        const float Coefficient = 2f;

        float chance = (level - 1) * Coefficient;
        chance = chance > 100 ? 200 - chance : chance;
        chance = Mathf.Clamp(chance, 0, 100);

        return chance > generalChance;
    }

    private Chunk GetRandomChunk(List<Chunk> chunks)
    {
        int randomIndex = Random.Range(0, chunks.Count);

        Chunk randomChunk = chunks[randomIndex];
        chunks.RemoveAt(randomIndex);

        return randomChunk;
    }

    private bool IsEasyChunk(Chunk chunk)
    {
        return chunk.Difficulty <= 8;
    }

    private bool IsHardChunk(Chunk chunk)
    {
        return chunk.Difficulty >= 18;
    }

    private bool IsMediumChunk(Chunk chunk)
    {
        return chunk.Difficulty >= 9 && chunk.Difficulty <= 17;
    }

    private void SortChunks(Chunk[] chunks)
    {
        _easyChunks = new List<Chunk>(chunks.Length);
        _mediumChunks = new List<Chunk>(chunks.Length);
        _hardChunks = new List<Chunk>(chunks.Length);

        foreach (Chunk chunk in chunks)
        {
            if (IsEasyChunk(chunk))
            {
                _easyChunks.Add(chunk);
            }
            else if (IsMediumChunk(chunk))
            {
                _mediumChunks.Add(chunk);
            }
            else if (IsHardChunk(chunk))
            {
                _hardChunks.Add(chunk);
            }
        }
    }
}