using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WoodsOfIdle
{
    [System.Serializable]
    public class SaveState
    {
        public string SaveName;

        public Dictionary<ItemType, int> StoredItems = new Dictionary<ItemType, int>();

        public Dictionary<int, FarmingNodeState> FarmingNodes = new Dictionary<int, FarmingNodeState>();
    }
}
