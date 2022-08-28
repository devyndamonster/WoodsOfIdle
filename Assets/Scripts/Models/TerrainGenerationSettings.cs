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
        public List<TerrainTileMapSettings> TileMapSettings = new List<TerrainTileMapSettings>();
    }

    [System.Serializable]
    public class PerlinNoiseSettings
    {
        public float Scale = 20;
        [Range(0, 1)] public float Strength = 1;
        public Vector2 Offset;
    }

    [System.Serializable]
    public class TerrainTileMapSettings
    {
        public CellType CellType;
        public Vector2 HeightRange;
    }
}


