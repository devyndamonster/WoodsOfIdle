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
        private SaveController saveController;


        /* TODO
         * - Have controller generate its own mesh and mesh components
         * - Pass values into init call for dependancy inversion
         * - Unit test this!
         */


        public TerrainTextureController Init(ITerrainService terrainService, SaveController saveController)
        {
            this.terrainService = terrainService;
            this.saveController = saveController;

            return this;
        }

        private void Start()
        {
            CellData[,] cells = terrainService.GenerateTerrainData(terrainSettings);
            GenerateTextureToMesh(cells);

            //Generate all the nodes and link them to the save data
        }

        public void GenerateTextureToMesh(CellData[,] cells)
        {
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
            meshRenderer.transform.localScale = new Vector3(settings.Size.x, 1, settings.Size.y) / 10;
        }
    }
}

