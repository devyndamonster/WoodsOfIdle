using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
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
        protected IAddressableLoaderService addressableLoaderService;
        protected static string nextSaveToOpen;

        protected event Action OnUpdated;
        protected event Action OnDestroyed;
        protected event Action<bool> OnApplicationPauseChanged;
        protected event Action<bool> OnApplicationFocusChanged;

        private void Awake()
        {
            addressableLoaderService = new AddressableLoaderService();
            IEnumerator routine = addressableLoaderService.LoadAssets(AssetCollection, (loadedAssetCollection) => InitScene(loadedAssetCollection));
            StartCoroutine(routine);
        }

        private void InitScene(AssetReferenceCollection assetCollection)
        {
            ISaveService saveService = new SaveService();
            IInventoryService inventoryService = new InventoryService();
            ITerrainService terrainService = new TerrainService();
            
            saveController = new SaveController(saveService);
            saveController.OpenSave(nextSaveToOpen);
            OnApplicationPauseChanged += saveController.OnApplicationPause;
            OnApplicationFocusChanged += saveController.OnApplicationPause;
            OnDestroyed += saveController.OnDestroy;

            terrainTextureController = new TerrainTextureController(terrainService, saveController, assetCollection);
            terrainTextureController.GenerateTerrain(TerrainSettings, TerrainMeshRenderer);

            inventoryController = new InventoryController(saveController, inventoryService, assetCollection.LoadedItemData, InventoryPanel);
            
            gameController = new GameController(saveController);
            OnUpdated += gameController.Update;
        }
        
        public static void SetNextSaveToOpen(string saveName)
        {
            nextSaveToOpen = saveName;
        }

        private void Update()
        {
            OnUpdated?.Invoke();
        }

        private void OnDestroy()
        {
            OnDestroyed?.Invoke();
        }

        private void OnApplicationPause(bool pause)
        {
            OnApplicationPauseChanged?.Invoke(pause);
        }

        private void OnApplicationFocus(bool focus)
        {
            OnApplicationFocusChanged?.Invoke(focus);
        }
    }
}
