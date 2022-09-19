using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WoodsOfIdle
{
    public interface ITerrainReceiver
    {
        public void ApplyTerrain(CellData[,] cellData, Dictionary<Vector2Int, GameObject> farmingNodePrefabs, TerrainGenerationSettings settings);
    }
}
