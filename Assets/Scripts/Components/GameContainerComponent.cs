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
        public TerrainGenerationSettings terrainSettings;
        public AssetReferenceCollectionScriptable assetReferences;

        protected DependancyContainer _container = new DependancyContainer();
        protected static string nextSaveToOpen;

        private void Start()
        {
            BindDependancies();
            
            LoadAssets((loadedAssets) =>
            {
                InitScene(loadedAssets);
                InitEvents();
            });
        }

        private void BindDependancies()
        {
            //Bind services
            _container.Bind<ISaveService, SaveService>();
            _container.Bind<IAddressableLoaderService, AddressableLoaderService>();
            _container.Bind<IInventoryService, InventoryService>();
            _container.Bind<IFarmingNodeService, FarmingNodeService>();
            _container.Bind<ITerrainService, TerrainService>();

            //Bind scene specific dependancies
            _container.Bind<TerrainGenerationSettings>(terrainSettings);
            _container.Bind<AssetReferenceCollection>(assetReferences.Value);
            _container.Bind<UIDocument>(uiDocument);
            
            //Bind controllers
            _container.Bind<SaveController>();
            _container.Bind<TerrainGenerationController>();
            _container.Bind<GameUIController>();

            //Bind inventory controllers
            _container.Bind<InventoryRelay>();
            _container.Bind<InventoryElementFactory>();
            _container.Bind<InventoryController>();
            _container.Bind<InventoryUIController>();
            
        }
        
        private void LoadAssets(Action<AssetReferenceCollection> onLoaded)
        {
            var assets = _container.Resolve<AssetReferenceCollection>();
            var assetLoader = _container.Resolve<AddressableLoaderService>();
            IEnumerator routine = assetLoader.LoadAssets(assets, onLoaded);
            StartCoroutine(routine);
        }

        private void InitScene(AssetReferenceCollection assets)
        {
            var saveController = _container.Resolve<SaveController>();
            saveController.OpenSave(nextSaveToOpen);

            var terrainSettings = _container.Resolve<TerrainGenerationSettings>();
            var terrainGenerationController = _container.Resolve<TerrainGenerationController>();
            var generatedTerrain = terrainGenerationController.GenerateTerrain(terrainSettings);
            saveController.CurrentSaveState.Cells = generatedTerrain.CellData;
            saveController.CurrentSaveState.FarmingNodes = generatedTerrain.FarmingNodes.ToDictionary(node => node.State.Position, node => node.State);
            
            //Apply the terrain
        }

        private void InitEvents()
        {
            //_container.CollectImplementationsOfType<IUpdateable>()>
        }
        
        //TODO why does this object need to know about the next save to open, get rid of this
        public static void SetNextSaveToOpen(string saveName)
        {
            nextSaveToOpen = saveName;
        }
    }
}
