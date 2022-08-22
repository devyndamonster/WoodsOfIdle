using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WoodsOfIdle
{
    public class TerrainBuilder
    {
        private int seed;
        private CellData[,] cells;

        public TerrainBuilder SetSeed(int seed)
        {
            this.seed = seed;
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
            //Randomize the colors of every cell
            for (int x = 0; x < cells.GetLength(0); x++)
            {
                for (int y = 0; y < cells.GetLength(1); y++)
                {
                    cells[x, y].Color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
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

