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

        public TerrainBuilder()
        {
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


        public TerrainBuilder RandomizeColors()
        {
            for (int x = 0; x < cells.GetLength(0); x++)
            {
                for (int y = 0; y < cells.GetLength(1); y++)
                {
                    cells[x, y].Color = new Color(random.NextFloat(), random.NextFloat(), random.NextFloat());
                }
            }

            return this;
        }

        public CellData[,] GetCells()
        {
            return cells;
        }
    }
}

