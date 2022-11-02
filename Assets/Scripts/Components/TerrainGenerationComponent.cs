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
            _meshFilter.mesh = _terrainMeshFactory.CreateTerrainMesh(_terrainData);
            _meshRenderer.material = _terrainSettings.TerrainMaterial;
            _meshRenderer.material.mainTexture = GetTextureFromTerrainData(_terrainData.CellData);
        }

        private Texture2D GetTextureFromTerrainData(CellData[,] cells)
        {
            int width = cells.GetLength(0);
            int height = cells.GetLength(1);

            Color[] colorMap = new Color[width * height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    colorMap[y * width + x] = cells[x, y].Color;
                }
            }

            Texture2D texture = new Texture2D(width, height);
            texture.SetPixels(colorMap);
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.filterMode = FilterMode.Point;
            texture.Apply();

            return texture;
        }
    }
}
