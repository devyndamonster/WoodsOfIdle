using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WoodsOfIdle
{
    public interface ITerrainReceiver
    {
        public void ApplyTerrain(TerrainGenerationData terrainData, TerrainGenerationSettings settings);
    }
}
