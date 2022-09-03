using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace WoodsOfIdle
{
    public class SceneManagerComponent : MonoBehaviour
    {
        public UIDocument InventoryPanel;
        public MeshRenderer TerrainMeshRenderer;
        public AssetReferenceCollection AssetCollection;
        public TerrainGenerationSettings TerrainSettings = new TerrainGenerationSettings();

        protected SaveController saveController;
        protected InventoryController inventoryController;
        protected GameController gameController;
        protected TerrainTextureController terrainTextureController;
        protected static string nextSaveToOpen;

        private void Awake()
        {
            ISaveService saveService = new SaveService();
            IInventoryService inventoryService = new InventoryService();
            ITerrainService terrainService = new TerrainService();

            //TODO Replace with addressables
            IEnumerable<ItemData> items = Resources.LoadAll<ItemData>("Items/Data");

            saveController = new SaveController(saveService);
            saveController.OpenSave(nextSaveToOpen);

            terrainTextureController = new TerrainTextureController(terrainService, saveController);
            terrainTextureController.GenerateTerrain(TerrainSettings, TerrainMeshRenderer);

            inventoryController = new InventoryController(saveController, inventoryService, items, InventoryPanel);
            gameController = new GameController(saveController);
            
            
        }

        public static void SetNextSaveToOpen(string saveName)
        {
            nextSaveToOpen = saveName;
        }

        private void Update()
        {
            gameController.Update();
        }

        public void OnDestroy()
        {
            saveController.OnDestroy();
        }

        public void OnApplicationPause(bool pause)
        {
            saveController.OnApplicationPause(pause);
        }

        public void OnApplicationFocus(bool focus)
        {
            saveController.OnApplicationFocus(focus);
        }
    }
}
