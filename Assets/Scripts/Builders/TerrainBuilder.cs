using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WoodsOfIdle
{
    public class TerrainBuilder
    {
        private int seed;
        private CellData[,] cells;
        private System.Random random;
        
        private List<Action<CellData, int, int>> cellBuildingActions;

        public TerrainBuilder()
        {
            cellBuildingActions = new List<Action<CellData, int, int>>();
            random = new System.Random(0);
        }

        public TerrainBuilder SetSeed(int seed)
        {
            this.seed = seed;
            random = new System.Random(seed);
            return this;
        }

        public TerrainBuilder CreateCells(int minX, int minY, int maxX, int maxY)
        {
            cells = new CellData[maxX - minX, maxY - minY];

            for (int x = minX; x < maxX; x++)
            {
                for (int y = minY; y < maxY; y++)
                {
                    cells[x, y] = new CellData();
                }
            }

            return this;
        }
        
        
        public TerrainBuilder GenerateCellHeightsFromPerlinNoise(float scale, float offset)
        {
            cellBuildingActions.Add((CellData cellData, int x, int y) =>
            {
                cellData.Height = GetPerlinNoise(x, y, scale, offset);
            });
            
            return this;
        }

        
        public TerrainBuilder MapHeightToColor()
        {
            cellBuildingActions.Add((CellData cellData, int x, int y) =>
            {
                cellData.Color = new Color(cellData.Height, cellData.Height, cellData.Height);
            });

            return this;
        }
        
        public TerrainBuilder RandomizeColors()
        {
            cellBuildingActions.Add((CellData cellData, int x, int y) =>
            {
                cellData.Color = new Color(random.NextFloat(), random.NextFloat(), random.NextFloat());
            });

            return this;
        }

        public CellData[,] GetCells()
        {
            int width = cells.GetLength(0);
            int height = cells.GetLength(1);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    cells[x, y] = GetCellDataAtPosition(x, y, cellBuildingActions);
                }
            }
            
            return cells;
        }

        private CellData GetCellDataAtPosition(int x, int y, List<Action<CellData, int, int>> actions)
        {
            CellData cellData = new CellData();
            
            foreach (Action<CellData, int, int> action in actions)
            {
                action(cellData, x, y);
            }

            return cellData;
        }


        private float GetPerlinNoise(int x, int y, float scale, float offset)
        {
            float sampleX = (x / scale) + offset;
            float sampleY = (y / scale) + offset;
            return Mathf.PerlinNoise(sampleX, sampleY);
        }
        
    }
}

