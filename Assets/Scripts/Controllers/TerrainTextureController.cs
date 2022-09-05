using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WoodsOfIdle
{
    public class TerrainTextureController
    {
        private ITerrainService terrainService;
        private SaveController saveController;
        private AssetReferenceCollection assetReferences;
        
        public TerrainTextureController(ITerrainService terrainService, SaveController saveController, AssetReferenceCollection assets)
        {
            this.terrainService = terrainService;
            this.saveController = saveController;
            this.assetReferences = assets;
        }

        public void GenerateTerrain(TerrainGenerationSettings settings, MeshRenderer targetMesh)
        {
            CellData[,] cells = terrainService.GenerateTerrainData(settings);
            GenerateTextureToMesh(settings, targetMesh, cells);
            GenerateFarmingNodes(settings, cells, assetReferences.LoadedFarmingNodePrefabs);
        }

        public void GenerateTextureToMesh(TerrainGenerationSettings settings, MeshRenderer targetMesh, CellData[,] cells)
        {
            Texture2D texture = terrainService.GetTextureFromTerrainData(cells);
            SetMeshTexture(texture, targetMesh);
            SetMeshScale(settings, targetMesh);
        }
        
        public void GenerateFarmingNodes(TerrainGenerationSettings settings, CellData[,] cells, List<GameObject> farmingNodePrefabs)
        {
            foreach (GameObject prefab in farmingNodePrefabs)
            {
                Debug.Log($"Spawning prefab: {prefab.name}");
                FarmingNodeComponent farmingNode = prefab.GetComponent<FarmingNodeComponent>();
                List<Vector2> spawnPositions = terrainService.GetSpawnPositionsForFarmingNode(farmingNode.Data, cells, settings.Seed);

                foreach (Vector2 position in spawnPositions)
                {
                    GameObject.Instantiate(prefab, new Vector3(position.x, 0, position.y), Quaternion.identity);
                }
            }
        }

        private void SetMeshTexture(Texture2D texture, MeshRenderer targetMesh)
        {
            targetMesh.sharedMaterial.mainTexture = texture;
        }

        private void SetMeshScale(TerrainGenerationSettings settings, MeshRenderer targetMesh)
        {
            targetMesh.transform.localScale = new Vector3(settings.Size.x, 1, settings.Size.y) / 10;
        }
    }
}

