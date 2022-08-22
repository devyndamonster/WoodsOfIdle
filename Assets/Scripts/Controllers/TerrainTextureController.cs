using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WoodsOfIdle
{
    public class TerrainTextureController : MonoBehaviour
    {
        public MeshRenderer meshRenderer;
        public TerrainGenerationSettings terrainSettings = new TerrainGenerationSettings();

        private ITerrainService terrainService = new TerrainService();

        private void Awake()
        {
            GenerateTextureToMesh();
        }

        public void GenerateTextureToMesh()
        {
            TerrainGenerationSettings settings = new TerrainGenerationSettings
            {
                MinX = 0,
                MinY = 0,
                MaxX = 100,
                MaxY = 100,
                Seed = 1
            };

            CellData[,] cells = terrainService.GenerateTerrainData(settings);
            Texture2D texture = terrainService.GetTextureFromTerrainData(cells);
            SetMeshTexture(texture);
        }

        private void SetMeshTexture(Texture2D texture)
        {
            meshRenderer.sharedMaterial.mainTexture = texture;
        }
    }
}

