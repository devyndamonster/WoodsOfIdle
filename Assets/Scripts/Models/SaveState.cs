using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WoodsOfIdle
{
    [System.Serializable]
    public class SaveState
    {
        public string SaveName;
        
        public Dictionary<string, FarmingNodeState> FarmingNodes = new Dictionary<string, FarmingNodeState>();

        public Dictionary<string, InventorySlotState> InventoryInSlots = new Dictionary<string, InventorySlotState>();
    }
}
