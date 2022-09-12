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
        protected List<FarmingNodeComponent> farmingNodes = new List<FarmingNodeComponent>();

        public GameController(SaveController saveController, IEnumerable<FarmingNodeComponent> farmingNodes)
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
            foreach(FarmingNodeComponent farmingNode in farmingNodes)
            {
                farmingNode.UpdateState();
            }
        }
    }
}
