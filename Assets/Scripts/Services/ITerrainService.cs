using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WoodsOfIdle
{
    public interface ITerrainService
    {
        public CellData[,] GenerateTerrainData(TerrainGenerationSettings settings);

        public IEnumerable<FarmingNodeController> GenerateFarmingNodeControllers(
            TerrainGenerationSettings settings,
            IFarmingNodeControllerFactory farmingNodeFactory,
            CellData[,] cells,
            IEnumerable<FarmingNodeData> farmingNodeData);
        
        public IEnumerable<FarmingNodeController> GetFarmingNodeControllersFromState(
            IFarmingNodeControllerFactory farmingNodeFactory,
            IEnumerable<FarmingNodeState> states);
        
        public Vector3 GetSpawnPositionOffset(TerrainGenerationSettings settings);

        public Vector3 GetWorldPositionFromCellPosition(TerrainGenerationSettings settings, Vector2Int cellPosition);
        
    }
}

