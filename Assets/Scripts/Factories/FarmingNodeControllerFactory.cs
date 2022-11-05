using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WoodsOfIdle
{
    public class FarmingNodeControllerFactory : IFarmingNodeControllerFactory
    {
        private IFarmingNodeService _farmingNodeService;
        private AssetReferenceCollection _assetReferences;
        private InventoryController _inventoryController;

        public FarmingNodeControllerFactory(IFarmingNodeService farmingNodeService, InventoryController inventoryController, AssetReferenceCollection assetReferences)
        {
            _farmingNodeService = farmingNodeService;
            _inventoryController = inventoryController;
            _assetReferences = assetReferences;
        }

        public FarmingNodeController CreateFarmingNodeController(FarmingNodeType nodeType, Vector2Int position)
        {
            var farmingNodeData = _assetReferences.LoadedFarmingNodeData[nodeType];
            var farmingNode = new FarmingNodeController(_farmingNodeService, farmingNodeData, position);
            farmingNode.Harvested += _inventoryController.OnItemQuantityChanged;
            return farmingNode;
        }

        public FarmingNodeController CreateFarmingNodeController(FarmingNodeState state)
        {
            var farmingNodeData = _assetReferences.LoadedFarmingNodeData[state.NodeType];
            var farmingNode = new FarmingNodeController(_farmingNodeService, farmingNodeData, state);
            farmingNode.Harvested += _inventoryController.OnItemQuantityChanged;
            return farmingNode;
        }
    }
}
