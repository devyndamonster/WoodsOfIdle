using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace WoodsOfIdle
{
    public class GameController : IUpdateReceiver
    {
        private TerrainGenerationData _terrainData;

        public GameController(TerrainGenerationData terrainData)
        {
            _terrainData = terrainData;
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
    }
}
