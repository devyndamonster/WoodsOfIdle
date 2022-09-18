using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WoodsOfIdle
{
    [System.Serializable]
    public class SaveState
    {
        public string SaveName;
        
        public Dictionary<Vector2Int, FarmingNodeState> FarmingNodes = new Dictionary<Vector2Int, FarmingNodeState>();

        public Dictionary<string, InventorySlotState> InventoryInSlots = new Dictionary<string, InventorySlotState>();

        public CellData[,] Cells = new CellData[0,0];
    }
}
