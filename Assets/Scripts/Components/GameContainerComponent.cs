using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace WoodsOfIdle
{
    public class GameContainerComponent : MonoBehaviour
    {
        public UIDocument uiDocument;
        public TerrainGenerationSettingsScriptable terrainSettings;
        public AssetReferenceCollectionScriptable assetReferences;

        public bool IsInitialized { get; private set; }

        private DependancyContainer _container = new DependancyContainer();
        private static string _nextSaveToOpen;

        private void Start()
        {
            LoadAssets((loadedAssets) =>
            {
                BindDependancies(loadedAssets);
                InitScene();
                InitEvents();
                
                IsInitialized = true;
            });
        }

        private void LoadAssets(Action<AssetReferenceCollection> onLoaded)
        {
            var assetLoader = new AddressableLoaderService();
            IEnumerator routine = assetLoader.LoadAssets(assetReferences.Value, onLoaded);
            StartCoroutine(routine);
        }

        private void BindDependancies(AssetReferenceCollection assets)
        {
            //Bind services
            _container.Bind<ISaveService, SaveService>();
            _container.Bind<IInventoryService, InventoryService>();
            _container.Bind<IFarmingNodeService, FarmingNodeService>();
            _container.Bind<ITerrainService, TerrainService>();
            
            //Bind scene specific dependancies
            _container.Bind<TerrainGenerationSettings>(terrainSettings.Value);
            _container.Bind<AssetReferenceCollection>(assets);
            _container.Bind<UIDocument>(uiDocument);
            
            //Bind save controller and load save state for upcoming bindings
            _container.Bind<SaveController>();
            _container.Resolve<SaveController>().OpenSave(_nextSaveToOpen);
            
            //Bind UI Factories
            _container.Bind<IHarvestOptionElementFactory, HarvestOptionElementFactory>();
            _container.Bind<IInventoryElementFactory, InventoryElementFactory>();
            
            _container.Bind<InventoryController>();
            _container.Bind<InventoryRelay>();
            _container.Bind<InventoryUIController>();

            //Bind Generated terrain data
            _container.Bind<IFarmingNodeControllerFactory, FarmingNodeControllerFactory>();
            _container.Bind<TerrainGenerationController>();
            var terrainData = _container.Resolve<TerrainGenerationController>().GenerateTerrain(terrainSettings.Value);
            _container.Resolve<SaveController>().UpdateSaveStateFromTerrainData(terrainData);
            _container.Bind<TerrainGenerationData>(terrainData);
            
            //Bind UI Controllers
            _container.Bind<GameController>();
            _container.Bind<GameRelay>();
            _container.Bind<GameUIController>();

            //Bind the controllers responsible for generating world terrain
            _container.Bind<ITerrainMeshFactory, TerrainMeshFactory>();
            _container.Bind<IFarmingNodeComponentFactory, FarmingNodeComponentFactory>();
            _container.BindComponent<TerrainGenerationComponent>();
        }
        
        private void InitScene()
        {
            _container.Resolve<TerrainGenerationComponent>().BuildTerrain();
        }

        private void InitEvents() 
        {
            GameEventComponent.Create()
                .WithReceivers(_container.CollectImplementationsOfType<IUpdateReceiver>())
                .WithReceivers(_container.CollectImplementationsOfType<IDestroyReceiver>())
                .WithReceivers(_container.CollectImplementationsOfType<IPauseReceiver>())
                .WithReceivers(_container.CollectImplementationsOfType<IFocusReceiver>());
        }
        
        //TODO why does this object need to know about the next save to open, get rid of this
        public static void SetNextSaveToOpen(string saveName)
        {
            _nextSaveToOpen = saveName;
        }
    }
}
