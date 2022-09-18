using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace WoodsOfIdle
{
    public class GameController
    {
        protected SaveController saveController;
        protected List<FarmingNodeController> farmingNodes = new List<FarmingNodeController>();

        public GameController(SaveController saveController, IEnumerable<FarmingNodeController> farmingNodes)
        {
            this.saveController = saveController;
            this.farmingNodes = farmingNodes.ToList();
        }

        public void Update()
        {
            UpdateFarmingNodes();
        }

        private void UpdateFarmingNodes()
        {
            foreach(FarmingNodeController farmingNode in farmingNodes)
            {
                farmingNode.UpdateState();
            }
        }
    }
}
