using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WoodsOfIdle
{
    [System.Serializable]
    public class SaveState
    {
        public string SaveName;

        public Dictionary<int, FarmingNodeState> FarmingNodes = new Dictionary<int, FarmingNodeState>();

        public Dictionary<string, InventorySlotState> InventoryInSlots = new Dictionary<string, InventorySlotState>();
    }
}
