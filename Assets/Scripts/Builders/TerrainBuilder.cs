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
        
        
        public TerrainBuilder GenerateCellHeightsFromPerlinNoise(float scale, float offset)
        {
            int width = cells.GetLength(0);
            int height = cells.GetLength(1);
            float[,] noiseMap = GetNoiseMap(width, height, scale, offset);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    cells[x, y].Height = noiseMap[x, y];
                    //Debug.Log(cells[x, y].Height);
                }
            }

            return this;
        }

        
        public TerrainBuilder MapHeightToColor()
        {
            int width = cells.GetLength(0);
            int height = cells.GetLength(1);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    cells[x, y].Color = new Color(cells[x, y].Height, cells[x, y].Height, cells[x, y].Height);
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
        
        private float[,] GetNoiseMap(int width, int height, float scale, float offset)
        {
            float[,] noiseMap = new float[width, height];
                
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    float sampleX = (x * scale) + offset;
                    float sampleY = (y * scale) + offset;
                    noiseMap[x, y] = Mathf.PerlinNoise(sampleX, sampleY);

                    Debug.Log($"Samplex: {sampleX}, Sampley: {sampleY}, Noise: {noiseMap[x, y]}");
                }
            }
            
            return noiseMap;
        }
        
    }
}

