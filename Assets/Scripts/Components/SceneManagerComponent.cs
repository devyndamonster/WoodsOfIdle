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
        protected GameUIController gameUIController;
        protected GameController gameController;
        protected TerrainGenerationController terrainGenerationController;
        protected IAddressableLoaderService addressableLoaderService;
        protected static string nextSaveToOpen;

        protected event Action OnUpdated;
        protected event Action OnDestroyed;
        protected event Action<bool> OnApplicationPauseChanged;
        protected event Action<bool> OnApplicationFocusChanged;

        private void Start()
        {
            addressableLoaderService = new AddressableLoaderService();
            IEnumerator routine = addressableLoaderService.LoadAssets(AssetCollection, (loadedAssetCollection) => InitScene(loadedAssetCollection));
            StartCoroutine(routine);
        }

        private void InitScene(AssetReferenceCollection assetCollection)
        {
            ISaveService saveService = new SaveService();
            IInventoryService inventoryService = new InventoryService();
            IFarmingNodeService farmingNodeService = new FarmingNodeService();
            ITerrainService terrainService = new TerrainService(farmingNodeService);
            IEnumerable<ITerrainReceiver> terrainReceivers = FindObjectsOfType<MonoBehaviour>().OfType<ITerrainReceiver>();

            saveController = new SaveController(saveService);
            saveController.OpenSave(nextSaveToOpen);
            OnApplicationPauseChanged += saveController.OnApplicationPause;
            OnApplicationFocusChanged += saveController.OnApplicationPause;
            OnDestroyed += saveController.OnDestroy;
            
            terrainGenerationController = new TerrainGenerationController(terrainService, saveController, assetCollection);
            TerrainGenerationController.TerrainResult generatedTerrainData = terrainGenerationController.GenerateTerrain(TerrainSettings);
            ApplyGeneratedTerrain(terrainReceivers, generatedTerrainData.CellData, generatedTerrainData.FarmingNodePrefabs);
            
            inventoryController = new InventoryController(saveController, inventoryService, assetCollection.LoadedItemData, InventoryPanel);
            gameUIController = new GameUIController(InventoryPanel, assetCollection.LoadedItemData, generatedTerrainData.FarmingNodes);

            gameController = new GameController(saveController, generatedTerrainData.FarmingNodes);
            OnUpdated += gameController.Update;
        }

        private void ApplyGeneratedTerrain(IEnumerable<ITerrainReceiver> receivers, CellData[,] cells, Dictionary<Vector2Int, GameObject> farmingNodePrefabs)
        {
            foreach (ITerrainReceiver terrainReceiver in receivers)
            {
                terrainReceiver.ApplyTerrain(cells, farmingNodePrefabs, TerrainSettings);
            }
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
