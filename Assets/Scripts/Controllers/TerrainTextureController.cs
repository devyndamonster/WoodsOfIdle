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
            CellData[,] cells = terrainService.GenerateTerrainData(terrainSettings);
            Texture2D texture = terrainService.GetTextureFromTerrainData(cells);
            SetMeshTexture(texture);
            SetMeshScale(terrainSettings);
        }

        private void SetMeshTexture(Texture2D texture)
        {
            meshRenderer.sharedMaterial.mainTexture = texture;
        }

        private void SetMeshScale(TerrainGenerationSettings settings)
        {
            meshRenderer.transform.localScale = new Vector3(settings.MaxX - settings.MinX, 1, settings.MaxY - settings.MinY) / 10;
        }
    }
}

