using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WoodsOfIdle
{
    public class TerrainGenerationData
    {
        public CellData[,] CellData;
        public IEnumerable<FarmingNodeController> FarmingNodes;
        public Dictionary<Vector2Int, GameObject> FarmingNodePrefabs;
    }
}
