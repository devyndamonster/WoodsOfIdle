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
        public AssetReferenceCollectionScriptable AssetCollection;
        public TerrainGenerationSettings TerrainSettings = new TerrainGenerationSettings();

        protected SaveController saveController;
        protected InventoryController inventoryController;
        protected GameUIController gameUIController;
        protected GameController gameController;
        protected TerrainGenerationController terrainGenerationController;
        protected IAddressableLoaderService addressableLoaderService;
        protected ISaveService saveService;
        protected IInventoryService inventoryService;
        protected IFarmingNodeService farmingNodeService;
        protected ITerrainService terrainService;
        protected static string nextSaveToOpen;
        protected bool isSceneInit;

        protected event Action OnUpdated;
        protected event Action OnDestroyed;
        protected event Action<bool> OnApplicationPauseChanged;
        protected event Action<bool> OnApplicationFocusChanged;

        private void Start()
        {
            InitServices();
            IEnumerator routine = addressableLoaderService.LoadAssets(AssetCollection.Value, (loadedAssetCollection) => InitScene(loadedAssetCollection));
            StartCoroutine(routine);
        }

        private void InitServices()
        {
            addressableLoaderService = new AddressableLoaderService();
            saveService = new SaveService();
            inventoryService = new InventoryService();
            farmingNodeService = new FarmingNodeService();
            terrainService = new TerrainService(farmingNodeService);
        }

        private void InitScene(AssetReferenceCollection assetCollection)
        {
            IEnumerable<ITerrainReceiver> terrainReceivers = FindObjectsOfType<MonoBehaviour>().OfType<ITerrainReceiver>();

            saveController = new SaveController(saveService);
            saveController.OpenSave(nextSaveToOpen);
            
            terrainGenerationController = new TerrainGenerationController(terrainService, saveController, assetCollection);
            var generatedTerrainData = terrainGenerationController.GenerateTerrain(TerrainSettings);
            saveController.CurrentSaveState.Cells = generatedTerrainData.CellData;
            saveController.CurrentSaveState.FarmingNodes = generatedTerrainData.FarmingNodes.ToDictionary(node => node.State.Position, node => node.State);
            ApplyGeneratedTerrain(terrainReceivers, generatedTerrainData.CellData, generatedTerrainData.FarmingNodePrefabs);
            
            inventoryController = new InventoryController(saveController, inventoryService, assetCollection.LoadedItemData, InventoryPanel);
            gameUIController = new GameUIController(InventoryPanel, assetCollection.LoadedItemData, generatedTerrainData.FarmingNodes);
            gameController = new GameController(saveController, generatedTerrainData.FarmingNodes);

            InitEvents();

            isSceneInit = true;
        }

        private void InitEvents()
        {
            OnApplicationPauseChanged += saveController.OnApplicationPause;
            OnApplicationFocusChanged += saveController.OnApplicationPause;
            OnDestroyed += saveController.OnDestroy;

            OnUpdated += gameController.Update;
        }

        private void ApplyGeneratedTerrain(IEnumerable<ITerrainReceiver> receivers, CellData[,] cells, Dictionary<Vector2Int, GameObject> farmingNodePrefabs)
        {
            foreach (ITerrainReceiver terrainReceiver in receivers)
            {
                terrainReceiver.ApplyTerrain(cells, farmingNodePrefabs, TerrainSettings);
            }
        }

        public bool IsSceneInitialized()
        {
            return isSceneInit;
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
