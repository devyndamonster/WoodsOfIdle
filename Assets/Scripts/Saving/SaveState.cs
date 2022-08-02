using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WoodsOfIdle
{
    [System.Serializable]
    public class SaveState
    {
        public string SaveName;

        public Dictionary<NodeType, int> StoredItems = new Dictionary<NodeType, int>();

        public Dictionary<int, FarmingNodeState> FarmingNodes = new Dictionary<int, FarmingNodeState>();
    }
}
