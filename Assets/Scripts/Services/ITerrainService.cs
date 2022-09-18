using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WoodsOfIdle
{
    public interface ITerrainService
    {
        public CellData[,] GenerateTerrainData(TerrainGenerationSettings settings);

        public List<FarmingNodeController> GenerateFarmingNodeControllers(TerrainGenerationSettings settings, CellData[,] cells, IEnumerable<FarmingNodeData> farmingNodeData);

        public List<FarmingNodeController> GetFarmingNodeControllersFromState(IEnumerable<FarmingNodeState> states, IDictionary<FarmingNodeType, FarmingNodeData> data);

        public List<Vector2Int> GetSpawnPositionsForFarmingNode(TerrainGenerationSettings settings, FarmingNodeData nodeData, CellData[,] cells);

        public List<Vector2Int> GetSpawnPositionsForFarmingNode(TerrainGenerationSettings settings, FarmingNodeData nodeData, CellData[,] cells, IEnumerable<Vector2Int> excludedPositions);

        public Vector3 GetSpawnPositionOffset(TerrainGenerationSettings settings);

        public Vector3 GetWorldPositionFromCellPosition(TerrainGenerationSettings settings, Vector2Int cellPosition);

        public int GetFarmingNodeGenerationSeed(int seed, FarmingNodeData nodeData);
    }
}

