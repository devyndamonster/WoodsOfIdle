using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WoodsOfIdle
{
    [System.Serializable]
    public class SaveState
    {
        public string SaveName;
        
        public IDictionary<Vector2Int, FarmingNodeState> FarmingNodes = new Dictionary<Vector2Int, FarmingNodeState>();

        public IDictionary<string, InventorySlotState> InventoryInSlots = new Dictionary<string, InventorySlotState>();
    }
}
