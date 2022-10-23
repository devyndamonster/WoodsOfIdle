using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace WoodsOfIdle
{
    public class GameController : IUpdateReceiver
    {
        public event Action<FarmingNodeController> OnFarmingNodeClicked;

        private TerrainGenerationData _terrainData;
        private Dictionary<Vector2Int, FarmingNodeController> _farmingNodes;

        public GameController(TerrainGenerationData terrainData)
        {
            _terrainData = terrainData;
            _farmingNodes = terrainData.FarmingNodes.ToDictionary(farmingNode => farmingNode.State.Position);
        }

        public void Update()
        {
            UpdateFarmingNodes();
        }

        private void UpdateFarmingNodes()
        {
            foreach(FarmingNodeController farmingNode in _terrainData.FarmingNodes)
            {
                farmingNode.UpdateState();
            }
        }

        public void HandleNodeClicked(Vector2Int nodePosition)
        {
            var farmingNode = _farmingNodes[nodePosition];
            OnFarmingNodeClicked?.Invoke(farmingNode);
        }
    }
}
