using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WoodsOfIdle
{
    public interface ITerrainMeshFactory
    {
        public Mesh CreateTerrainMesh(TerrainGenerationData terrainData);
    }
}
