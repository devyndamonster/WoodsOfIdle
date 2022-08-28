using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace WoodsOfIdle
{
    [System.Serializable]
    public class TerrainGenerationSettings
    {
        public Vector2Int Origin;
        public Vector2Int Size;
        public int Seed;
        public List<PerlinNoiseSettings> HeightMapSettings = new List<PerlinNoiseSettings>();
    }

    [System.Serializable]
    public class PerlinNoiseSettings
    {
        public float Scale;
        public Vector2 Offset;
    }
}


