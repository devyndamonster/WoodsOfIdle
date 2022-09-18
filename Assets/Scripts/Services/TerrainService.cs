using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WoodsOfIdle
{
    public class TerrainService : ITerrainService
    {
        private IFarmingNodeService farmingNodeService;

        public TerrainService(IFarmingNodeService farmingNodeService)
        {
            this.farmingNodeService = farmingNodeService;
        }
        
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
        
        public List<FarmingNodeController> GenerateFarmingNodeControllers(TerrainGenerationSettings settings, CellData[,] cells, IEnumerable<FarmingNodeData> farmingNodeData)
        {
            List<FarmingNodeController> farmingNodes = new List<FarmingNodeController>();

            foreach (var nodeData in farmingNodeData)
            {
                var spawnedNodes = GenerateFarmingNodeControllers(settings, cells, nodeData, farmingNodes.Select(node => node.State.Position));
                farmingNodes.AddRange(spawnedNodes);
            }

            return farmingNodes;
        }

        private List<FarmingNodeController> GenerateFarmingNodeControllers(TerrainGenerationSettings settings, CellData[,] cells,  FarmingNodeData data, IEnumerable<Vector2Int> excludedPositions)
        {
            List<FarmingNodeController> farmingNodes = new List<FarmingNodeController>();
            List<Vector2Int> spawnPositions = GetSpawnPositionsForFarmingNode(settings, data, cells, excludedPositions);

            foreach (Vector2Int cellPosition in spawnPositions)
            {
                FarmingNodeController nodeController = new FarmingNodeController(farmingNodeService, data, cellPosition);
                farmingNodes.Add(nodeController);
            }

            return farmingNodes;
        }

        public List<FarmingNodeController> GetFarmingNodeControllersFromState(IEnumerable<FarmingNodeState> states, IDictionary<FarmingNodeType, FarmingNodeData> data)
        {
            return states
                .Select(state => new FarmingNodeController(farmingNodeService, data[state.NodeType], state))
                .ToList();
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

