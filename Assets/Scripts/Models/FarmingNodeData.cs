using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WoodsOfIdle
{
    [System.Serializable]
    public class FarmingNodeData
    {
        public FarmingNodeType NodeType;
        public float DefaultTimeToHarvest = 1;
        public float SpawnChance = 0.001f;
        public List<CellType> AllowedCellTypes = new List<CellType>();
        public List<FarmingNodeItemOption> HarvestableItems = new List<FarmingNodeItemOption>();
    }

    [System.Serializable]
    public class FarmingNodeItemOption
    {
        public ItemType ItemType;
        public float ChanceToFarm;
    }
}

