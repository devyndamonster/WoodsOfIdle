using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

            foreach (TileMapSettings tileMapSettings in settings.TileMapSettings)
            {
                terrainBuilder.MapValueRangesToCellType(tileMapSettings.CellType, tileMapSettings.HeightRange);
            }
            
            foreach (TileColorSettings tileColorSettings in settings.TileColorSettings)
            {
                terrainBuilder.MapCellTypeToColor(tileColorSettings.CellType, tileColorSettings.Color);
            }

            return terrainBuilder.GetCells();
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

        public List<Vector2> GetSpawnPositionsForFarmingNode(TerrainGenerationSettings settings, FarmingNodeData nodeData, CellData[,] cells)
        {
            List<Vector2> spawnPositions = new List<Vector2>();
            System.Random random = new System.Random(settings.Seed);
            
            for(int x = 0; x < cells.GetLength(0); x++)
            {
                for(int y = 0; y < cells.GetLength(1); y++)
                {
                    if (nodeData.AllowedCellTypes.Contains(cells[x, y].Type) && nodeData.SpawnChance > random.NextFloat())
                    {
                        spawnPositions.Add(new Vector2(x + (settings.CellSize / 2), y + (settings.CellSize / 2)));
                    }
                }
            }
            
            return spawnPositions;
        }
    }
}

