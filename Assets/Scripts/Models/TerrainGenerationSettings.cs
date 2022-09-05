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
        public float CellSize = 1;
        public int Seed;
        public List<PerlinNoiseSettings> HeightMapSettings = new List<PerlinNoiseSettings>();
        public List<TileMapSettings> TileMapSettings = new List<TileMapSettings>();
        public List<TileColorSettings> TileColorSettings = new List<TileColorSettings>();
    }

    [System.Serializable]
    public class PerlinNoiseSettings
    {
        public float Scale = 20;
        [Range(0, 1)] public float Strength = 1;
        public Vector2 Offset;
    }

    [System.Serializable]
    public class TileMapSettings
    {
        public CellType CellType;
        public Vector2 HeightRange;
    }

    [System.Serializable]
    public class TileColorSettings
    {
        public CellType CellType;
        public Color Color;
    }
}


