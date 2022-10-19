using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WoodsOfIdle
{
    public class TerrainGenerationController
    {
        private ITerrainService terrainService;
        private SaveController saveController;
        private AssetReferenceCollection assetReferences;
        

        public TerrainGenerationController(ITerrainService terrainService, SaveController saveController, AssetReferenceCollection assetReferences)
        {
            this.terrainService = terrainService;
            this.saveController = saveController;
            this.assetReferences = assetReferences;
        }
        
        public TerrainGenerationData GenerateTerrain(TerrainGenerationSettings settings)
        {
            CellData[,] cells = GetGeneratedCellData(settings);
            List<FarmingNodeController> farmingNodeControllers = GetGeneratedFarmingNodeControllers(cells, settings);
            Dictionary<Vector2Int, GameObject> farmingNodePrefabs = GetGeneratedFarmingNodePrefabs(farmingNodeControllers);

            return new TerrainGenerationData { CellData = cells, FarmingNodes = farmingNodeControllers, FarmingNodePrefabs = farmingNodePrefabs };
        }

        private CellData[,] GetGeneratedCellData(TerrainGenerationSettings settings)
        {
            CellData[,] cells;

            if (saveController.CurrentSaveState.Cells != null && saveController.CurrentSaveState.Cells.Length > 0)
            {
                cells = saveController.CurrentSaveState.Cells;
            }
            else
            {
                cells = terrainService.GenerateTerrainData(settings);
            }
            
            return cells;
        }

        private List<FarmingNodeController> GetGeneratedFarmingNodeControllers(CellData[,] cells, TerrainGenerationSettings settings)
        {
            List<FarmingNodeController> farmingNodeControllers;
            
            if (saveController.CurrentSaveState.FarmingNodes != null && saveController.CurrentSaveState.FarmingNodes.Count() > 0)
            {
                Debug.Log("Loading farming nodes from save");
                farmingNodeControllers = terrainService.GetFarmingNodeControllersFromState(saveController.CurrentSaveState.FarmingNodes.Values, assetReferences.LoadedFarmingNodeData);
            }
            else
            {
                Debug.Log("Generating farming nodes: " + assetReferences.LoadedFarmingNodeData.Count());
                farmingNodeControllers = terrainService.GenerateFarmingNodeControllers(settings, cells, assetReferences.LoadedFarmingNodeData.Values);
            }
            
            return farmingNodeControllers;
        }
        
        private Dictionary<Vector2Int, GameObject> GetGeneratedFarmingNodePrefabs(List<FarmingNodeController> farmingNodeControllers)
        {
            Dictionary<Vector2Int, GameObject> farmingNodePrefabs = farmingNodeControllers
                .ToDictionary(controller => controller.State.Position, controller => assetReferences.LoadedFarmingNodePrefabs[controller.Data.NodeType]);

            return farmingNodePrefabs;
        }
    }
}

