using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace WoodsOfIdle
{
    public class GameUIController
    {
        protected UIDocument gameUIPanel;
        protected Dictionary<ItemType, ItemData> itemData;
        protected Dictionary<Vector2Int, FarmingNodeController> farmingNodes;

        public GameUIController(UIDocument gameUIPanel, Dictionary<ItemType, ItemData> itemData, IEnumerable<FarmingNodeController> farmingNodes)
        {
            this.gameUIPanel = gameUIPanel;
            this.itemData = itemData;
            this.farmingNodes = farmingNodes.ToDictionary(node => node.State.Position);
        }
        
        
    }
}
