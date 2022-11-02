using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        private List<TerrainGenerationStep> terrainGenerationSteps;

        public TerrainBuilder(Vector2Int origin, Vector2Int size, int seed)
        {
            this.origin = origin;
            this.size = size;

            terrainGenerationSteps = new List<TerrainGenerationStep>();
            random = new System.Random(seed);
            globalOffset = new Vector2(random.NextFloat(100000), random.NextFloat(100000));
            
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
        
        
        public TerrainBuilder AddPerlinNoiseToHeight(float scale, float strength, Vector2 offset)
        {
            AddTerrainGenerationStep((CellData cellData, int x, int y) =>
            {
                cellData.Height += GetPerlinNoise(x, y, scale, offset) * strength;
                Debug.Log($"X: {x}, Y: {y}, offset: {offset}, globaloffset: {globalOffset}, strength: {strength}, scale:{scale}, Height: {cellData.Height}");
            });
            
            return this;
        }

        
        public TerrainBuilder MapHeightToColor()
        {
            AddTerrainGenerationStep((CellData cellData, int x, int y) =>
            {
                cellData.Color = new Color(cellData.Height, cellData.Height, cellData.Height);
            });

            return this;
        }
        
        public TerrainBuilder RandomizeColors()
        {
            AddTerrainGenerationStep((CellData cellData, int x, int y) =>
            {
                cellData.Color = new Color(random.NextFloat(), random.NextFloat(), random.NextFloat());
            });

            return this;
        }

        public TerrainBuilder MapValueRangesToCellType(CellType type, Vector2 heightRange)
        {
            AddTerrainGenerationStep((CellData cellData, int x, int y) =>
            {
                if (cellData.Height >= heightRange.x && cellData.Height <= heightRange.y)
                {
                    cellData.Type = type;
                }
            });

            return this;
        }
        
        public TerrainBuilder MapCellTypeToColor(CellType type, Color color)
        {
            AddTerrainGenerationStep((CellData cellData, int x, int y) =>
            {
                if (cellData.Type == type)
                {
                    cellData.Color = color;
                }
            });
            
            return this;
        }

        public TerrainBuilder NormalizeCellHeights()
        {
            AddTerrainGenerationStep((CellData[,] cells) =>
            {
                float minHeight = cells.Cast<CellData>().Min(cell => cell.Height);
                float maxHeight = cells.Cast<CellData>().Max(cell => cell.Height);

                foreach (CellData cell in cells)
                {
                    cell.Height = (cell.Height - minHeight) / (maxHeight - minHeight);
                }
            });

            return this;
        }

        public CellData[,] GetCells()
        {
            foreach (TerrainGenerationStep step in terrainGenerationSteps)
            {
                step.ExecuteStep(cells);
            }

            return cells;
        }
        
        private float GetPerlinNoise(int x, int y, float scale, Vector2 offset)
        {
            float sampleX = (x + origin.x + globalOffset.x + offset.x) / scale;
            float sampleY = (y + origin.y + globalOffset.y + offset.y) / scale;
            Debug.Log($"samplex: {sampleX}, sampley: {sampleY}");
            return Mathf.PerlinNoise(sampleX, sampleY);
        }

        private void AddTerrainGenerationStep(Action<CellData, int, int> action)
        {
            if(terrainGenerationSteps.Count == 0)
            {
                TerrainGenerationStep step = new TerrainGenerationStep();
                step.singleCellActions.Add(action);
                terrainGenerationSteps.Add(step);
            }

            else if(terrainGenerationSteps.Last().multiCellActions.Count == 0)
            {
                terrainGenerationSteps.Last().singleCellActions.Add(action);
            }

            else
            {
                TerrainGenerationStep step = new TerrainGenerationStep();
                step.singleCellActions.Add(action);
                terrainGenerationSteps.Add(step);
            }
        }


        private void AddTerrainGenerationStep(Action<CellData[,]> action)
        {
            if (terrainGenerationSteps.Count == 0)
            {
                TerrainGenerationStep step = new TerrainGenerationStep();
                step.multiCellActions.Add(action);
                terrainGenerationSteps.Add(step);
            }

            else if (terrainGenerationSteps.Last().singleCellActions.Count == 0)
            {
                terrainGenerationSteps.Last().multiCellActions.Add(action);
            }

            else
            {
                TerrainGenerationStep step = new TerrainGenerationStep();
                step.multiCellActions.Add(action);
                terrainGenerationSteps.Add(step);
            }
        }


        private class TerrainGenerationStep
        {
            public List<Action<CellData, int, int>> singleCellActions = new List<Action<CellData, int, int>>();
            public List<Action<CellData[,]>> multiCellActions = new List<Action<CellData[,]>>();

            public void ExecuteStep(CellData[,] cells)
            {
                foreach (Action<CellData[,]> action in multiCellActions)
                {
                    action(cells);
                }

                if(singleCellActions.Count > 0)
                {
                    for (int x = 0; x < cells.GetLength(0); x++)
                    {
                        for (int y = 0; y < cells.GetLength(1); y++)
                        {
                            foreach (Action<CellData, int, int> action in singleCellActions)
                            {
                                action(cells[x, y], x, y);
                            }
                        }
                    }
                }
            }
        }
        
    }
}

