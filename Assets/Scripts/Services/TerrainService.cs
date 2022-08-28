using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WoodsOfIdle
{
    public class TerrainService : ITerrainService
    {
        public CellData[,] GenerateTerrainData(TerrainGenerationSettings settings)
        {
            TerrainBuilder terrainBuilder = new TerrainBuilder(settings.Origin, settings.Size, settings.Seed);

            foreach (PerlinNoiseSettings heightMapSetting in settings.HeightMapSettings)
            {
                terrainBuilder.AddPerlinNoiseToHeight(heightMapSetting.Scale, heightMapSetting.Strength, heightMapSetting.Offset);
            }

            return terrainBuilder
                .NormalizeCellHeights()
                .MapHeightToColor()
                .GetCells();
        }
        
        public Texture2D GetTextureFromTerrainData(CellData[,] cells)
        {
            int width = cells.GetLength(0);
            int height = cells.GetLength(1);

            Color[] colorMap = new Color[width * height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    colorMap[y * width + x] = cells[x, y].Color;
                }
            }

            Texture2D texture = new Texture2D(width, height);
            texture.SetPixels(colorMap);
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.filterMode = FilterMode.Point;
            texture.Apply();

            return texture;
        }
    }
}

