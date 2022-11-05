using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WoodsOfIdle
{
    public class TerrainGenerationController
    {
        private ITerrainService _terrainService;
        private IFarmingNodeControllerFactory _farmingNodeFactory;
        private SaveController _saveController;
        private AssetReferenceCollection _assetReferences;

        public TerrainGenerationController(
            ITerrainService terrainService,
            IFarmingNodeControllerFactory farmingNodeFactory,
            SaveController saveController,
            AssetReferenceCollection assetReferences)
        {
            _terrainService = terrainService;
            _farmingNodeFactory = farmingNodeFactory;
            _saveController = saveController;
            _assetReferences = assetReferences;
        }
        
        public TerrainGenerationData GenerateTerrain(TerrainGenerationSettings settings)
        {
            var cells = GetGeneratedCellData(settings);
            var farmingNodeControllers = GetGeneratedFarmingNodeControllers(cells, settings);
            var farmingNodePrefabs = GetGeneratedFarmingNodePrefabs(farmingNodeControllers);

            return new TerrainGenerationData { CellData = cells, FarmingNodes = farmingNodeControllers.ToList(), FarmingNodePrefabs = farmingNodePrefabs };
        }

        private CellData[,] GetGeneratedCellData(TerrainGenerationSettings settings)
        {
            CellData[,] cells;

            if (_saveController.CurrentSaveState.Cells != null && _saveController.CurrentSaveState.Cells.Length > 0)
            {
                cells = _saveController.CurrentSaveState.Cells;
            }
            else
            {
                cells = _terrainService.GenerateTerrainData(settings);
            }
            
            return cells;
        }

        private IEnumerable<FarmingNodeController> GetGeneratedFarmingNodeControllers(CellData[,] cells, TerrainGenerationSettings settings)
        {
            if (_saveController.CurrentSaveState.FarmingNodes != null && _saveController.CurrentSaveState.FarmingNodes.Count() > 0)
            {
                return _terrainService.GetFarmingNodeControllersFromState(_farmingNodeFactory, _saveController.CurrentSaveState.FarmingNodes.Values);
            }
            else
            {
                return _terrainService.GenerateFarmingNodeControllers(settings, _farmingNodeFactory, cells, _assetReferences.LoadedFarmingNodeData.Values);
            }
        }
        
        private Dictionary<Vector2Int, GameObject> GetGeneratedFarmingNodePrefabs(IEnumerable<FarmingNodeController> farmingNodeControllers)
        {
            Dictionary<Vector2Int, GameObject> farmingNodePrefabs = farmingNodeControllers
                .ToDictionary(
                    controller => controller.State.Position, 
                    controller => _assetReferences.LoadedFarmingNodePrefabs[controller.Data.NodeType]);

            return farmingNodePrefabs;
        }
    }
}

