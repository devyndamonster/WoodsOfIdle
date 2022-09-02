using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WoodsOfIdle
{
    public class TerrainTextureController
    {
        private ITerrainService terrainService;
        private SaveController saveController;
        
        public TerrainTextureController(ITerrainService terrainService, SaveController saveController)
        {
            this.terrainService = terrainService;
            this.saveController = saveController;
        }

        public void GenerateTerrain(TerrainGenerationSettings settings, MeshRenderer targetMesh)
        {
            CellData[,] cells = terrainService.GenerateTerrainData(settings);
            GenerateTextureToMesh(settings, targetMesh, cells);
            GenerateFarmingNodes(settings, cells);
        }

        public void GenerateTextureToMesh(TerrainGenerationSettings settings, MeshRenderer targetMesh, CellData[,] cells)
        {
            Texture2D texture = terrainService.GetTextureFromTerrainData(cells);
            SetMeshTexture(texture, targetMesh);
            SetMeshScale(settings, targetMesh);
        }

        public void GenerateFarmingNodes(TerrainGenerationSettings settings, CellData[,] cells)
        {
            
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

