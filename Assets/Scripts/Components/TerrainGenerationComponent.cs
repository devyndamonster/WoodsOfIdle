using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WoodsOfIdle
{
    public class TerrainGenerationComponent : MonoBehaviour
    {
        private TerrainGenerationData _terrainData;
        private TerrainGenerationSettings _terrainSettings;
        private IFarmingNodeComponentFactory _farmingNodeFactory;
        private ITerrainMeshFactory _terrainMeshFactory;
        private MeshFilter _meshFilter;
        private MeshRenderer _meshRenderer;

        [InjectDependencies]
        public void InjectDependancies(
            TerrainGenerationData terrainData,
            TerrainGenerationSettings terrainSettings,
            IFarmingNodeComponentFactory farmingNodeFactory,
            ITerrainMeshFactory terrainMeshFactory)
        {
            _terrainData = terrainData;
            _terrainSettings = terrainSettings;
            _farmingNodeFactory = farmingNodeFactory;
            _terrainMeshFactory = terrainMeshFactory;
            _meshFilter = gameObject.AddComponent<MeshFilter>();
            _meshRenderer = gameObject.AddComponent<MeshRenderer>();
        }

        public void BuildTerrain()
        {
            
        }
    }
}
