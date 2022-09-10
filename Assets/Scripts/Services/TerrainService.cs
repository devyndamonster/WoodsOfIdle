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

        public List<Vector2Int> GetSpawnPositionsForFarmingNode(TerrainGenerationSettings settings, FarmingNodeData nodeData, CellData[,] cells)
        {
            List<Vector2Int> spawnPositions = new List<Vector2Int>();
            System.Random random = new System.Random(settings.Seed);
            
            for(int x = 0; x < cells.GetLength(0); x++)
            {
                for(int y = 0; y < cells.GetLength(1); y++)
                {
                    if (nodeData.AllowedCellTypes.Contains(cells[x, y].Type) && nodeData.SpawnChance > random.NextFloat())
                    {
                        spawnPositions.Add(new Vector2Int(x, y));
                    }
                }
            }
            
            return spawnPositions;
        }
        
        public Vector3 GetSpawnPositionOffset(TerrainGenerationSettings settings)
        {
            return new Vector3(settings.CellSize / 2f, 0, settings.CellSize / 2f);
        }

        public Vector3 GetWorldPositionFromCellPosition(TerrainGenerationSettings settings, Vector2Int cellPosition)
        {
            return new Vector3(cellPosition.x * settings.CellSize, 0, cellPosition.y * settings.CellSize);
        }
    }
}

