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
        
        public IEnumerable<FarmingNodeController> GenerateFarmingNodeControllers(
            TerrainGenerationSettings settings, 
            IFarmingNodeControllerFactory farmingNodeFactory, 
            CellData[,] cells, 
            IEnumerable<FarmingNodeData> farmingNodeData)
        {
            List<FarmingNodeController> farmingNodes = new List<FarmingNodeController>();
            
            foreach (var nodeData in farmingNodeData)
            {
                var excludedSpawnPositions = farmingNodes.Select(node => node.State.Position);
                var selectedSpawnPositions = GetSpawnPositionsForFarmingNode(settings, nodeData, cells, excludedSpawnPositions);
                var farmingNodeControllers = selectedSpawnPositions.Select(position => farmingNodeFactory.CreateFarmingNodeController(nodeData.NodeType, position));
                farmingNodes.AddRange(farmingNodeControllers);
            }

            return farmingNodes;
        }

        public IEnumerable<FarmingNodeController> GetFarmingNodeControllersFromState(
            IFarmingNodeControllerFactory farmingNodeFactory,
            IEnumerable<FarmingNodeState> states)
        {
            return states.Select(state => farmingNodeFactory.CreateFarmingNodeController(state));
        }
        
        private List<Vector2Int> GetSpawnPositionsForFarmingNode(TerrainGenerationSettings settings, FarmingNodeData nodeData, CellData[,] cells, IEnumerable<Vector2Int> excludedPositions)
        {
            var spawnPositions = new List<Vector2Int>();
            var seed = GetFarmingNodeGenerationSeed(settings.Seed, nodeData);
            System.Random random = new System.Random(seed);

            for (int x = 0; x < cells.GetLength(0); x++)
            {
                for (int y = 0; y < cells.GetLength(1); y++)
                {
                    if (nodeData.AllowedCellTypes.Contains(cells[x, y].Type) && nodeData.SpawnChance > random.NextFloat() && !excludedPositions.Any(pos => pos.x == x && pos.y == y))
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

        private int GetFarmingNodeGenerationSeed(int seed, FarmingNodeData nodeData)
        {
            return seed + (int)nodeData.NodeType;
        }
    }
}

