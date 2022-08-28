using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WoodsOfIdle
{
    public class TerrainBuilder
    {
        private System.Random random;
        
        private CellData[,] cells;
        private Vector2 globalOffset;
        private Vector2Int origin;
        private Vector2Int size;

        private List<Action<CellData, int, int>> cellBuildingActions;

        public TerrainBuilder(Vector2Int origin, Vector2Int size, int seed)
        {
            this.origin = origin;
            this.size = size;

            cellBuildingActions = new List<Action<CellData, int, int>>();
            random = new System.Random(seed);
            globalOffset = new Vector2(random.Next(100000), random.Next(100000));
            
            CreateCells(size);
        }

        private TerrainBuilder CreateCells(Vector2Int size)
        {
            cells = new CellData[size.x, size.y];

            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    cells[x, y] = new CellData();
                }
            }

            return this;
        }
        
        
        public TerrainBuilder AddPerlinNoiseToHeight(float scale, Vector2 offset)
        {
            cellBuildingActions.Add((CellData cellData, int x, int y) =>
            {
                cellData.Height += GetPerlinNoise(x, y, scale, offset);
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
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
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

        
        private float GetPerlinNoise(int x, int y, float scale, Vector2 offset)
        {
            float sampleX = (x + origin.x + globalOffset.x + offset.x) / scale;
            float sampleY = (y + origin.y + globalOffset.y + offset.y) / scale;
            return Mathf.PerlinNoise(sampleX, sampleY);
        }
        
    }
}

