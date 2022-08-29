using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WoodsOfIdle
{
    [CreateAssetMenu(fileName = "FarmingNodeData", menuName = "Data/FarmingNodeData")]
    public class FarmingNodeData : ScriptableObject
    {
        public FarmingNodeType NodeType;
        public List<FarmingNodeItemOption> HarvestableItems = new List<FarmingNodeItemOption>();

        [System.Serializable]
        public class FarmingNodeItemOption
        {
            public ItemType ItemType;
            public float ChanceToFarm;
        }
    }
}

