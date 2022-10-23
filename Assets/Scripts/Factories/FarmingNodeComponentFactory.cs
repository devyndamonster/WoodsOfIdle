using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WoodsOfIdle
{
    public class FarmingNodeComponentFactory
    {
        private GameController _gameController;
        private AssetReferenceCollection _assetReferences;
        
        public FarmingNodeComponentFactory(GameController gameController, AssetReferenceCollection assetReferences)
        {
            _gameController = gameController;
            _assetReferences = assetReferences;
        }

        public FarmingNodeComponent Create(FarmingNodeController farmingNodeController)
        {
            GameObject farmingNodePrefab = GameObject.Instantiate(_assetReferences.LoadedFarmingNodePrefabs[farmingNodeController.State.NodeType]);
            FarmingNodeComponent farmingNodeComponent = farmingNodePrefab.GetComponent<FarmingNodeComponent>();
            farmingNodeComponent.NodeClicked += _gameController.HandleNodeClicked;

            return farmingNodeComponent;
        }
    }
}
