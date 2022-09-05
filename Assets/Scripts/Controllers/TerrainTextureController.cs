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
            SetMeshPosition(settings, targetMesh);
        }
        

        /* TODO
         * This needs to be refactored to do the following, but in seperate parts:
         * - Gets list of spawn positions for farming nodes
         * - Spawns the nodes
         * - Adds the spawned node ID to the cell data\
         * - - - - Maybe the cells shouldn't know about that node that's in it? Node knows what cell it is in?
         * - - - - Need data structure to quickly know if a node is inside a given cell
         */
        
        public void GenerateFarmingNodes(TerrainGenerationSettings settings, CellData[,] cells, List<GameObject> farmingNodePrefabs)
        {
            foreach (GameObject prefab in farmingNodePrefabs)
            {
                FarmingNodeComponent prefabComp = prefab.GetComponent<FarmingNodeComponent>();
                SpawnFarmingNodePrefab(prefabComp, cells, settings);
            }
        }
        
        private void SpawnFarmingNodePrefab(FarmingNodeComponent prefab, CellData[,] cells, TerrainGenerationSettings settings)
        {
            List<Vector2> spawnPositions = terrainService.GetSpawnPositionsForFarmingNode(settings, prefab.Data, cells);
            
            foreach (Vector2 position in spawnPositions)
            {
                GameObject node = GameObject.Instantiate(prefab.gameObject, new Vector3(position.x, 0, position.y), Quaternion.identity);
                FarmingNodeComponent nodeComp = node.GetComponent<FarmingNodeComponent>();

                nodeComp.State.NodeId = position.ToString("F0");
                nodeComp.ConnectToSaveState(saveController.CurrentSaveState);
            }
        }

        private void SetMeshTexture(Texture2D texture, MeshRenderer targetMesh)
        {
            targetMesh.sharedMaterial.mainTexture = texture;
        }

        private void SetMeshScale(TerrainGenerationSettings settings, MeshRenderer targetMesh)
        {
            targetMesh.transform.localScale = new Vector3(settings.Size.x, 1, settings.Size.y) * settings.CellSize / 10;
        }

        private void SetMeshPosition(TerrainGenerationSettings settings, MeshRenderer targetMesh)
        {
            targetMesh.gameObject.transform.position = new Vector3(settings.Origin.x + settings.Size.x / 2, 0, settings.Origin.y + settings.Size.y / 2);
        }
    }
}

