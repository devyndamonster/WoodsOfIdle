using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WoodsOfIdle
{
    public class TerrainTextureController
    {
        public List<FarmingNodeComponent> FarmingNodes { get; private set; }
        
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
        
        public void GenerateFarmingNodes(TerrainGenerationSettings settings, CellData[,] cells, List<GameObject> farmingNodePrefabs)
        {
            FarmingNodes = new List<FarmingNodeComponent>();

            foreach (GameObject prefab in farmingNodePrefabs)
            {
                FarmingNodeComponent prefabComp = prefab.GetComponent<FarmingNodeComponent>();
                var spawnedNodes = SpawnFarmingNodePrefabs(prefabComp, cells, settings);
                FarmingNodes.AddRange(spawnedNodes);
            }
        }
        
        private List<FarmingNodeComponent> SpawnFarmingNodePrefabs(FarmingNodeComponent prefab, CellData[,] cells, TerrainGenerationSettings settings)
        {
            List<FarmingNodeComponent> farmingNodes = new List<FarmingNodeComponent>();
            
            int nodeSeed = settings.Seed + (int)prefab.Data.NodeType;
            List<Vector2Int> spawnPositions = terrainService.GetSpawnPositionsForFarmingNode(nodeSeed, prefab.Data, cells);
            Vector3 spawnOffset = terrainService.GetSpawnPositionOffset(settings);
            
            foreach (Vector2Int cellPosition in spawnPositions)
            {
                Vector3 spawnPosition = terrainService.GetWorldPositionFromCellPosition(settings, cellPosition);
                GameObject node = GameObject.Instantiate(prefab.gameObject, spawnPosition + spawnOffset, Quaternion.identity);
                FarmingNodeComponent nodeComp = node.GetComponent<FarmingNodeComponent>();
                farmingNodes.Add(nodeComp);

                nodeComp.State.Position = cellPosition;
                nodeComp.ConnectToSaveState(saveController.CurrentSaveState);
            }

            return farmingNodes;
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

