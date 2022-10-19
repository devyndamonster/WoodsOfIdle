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
            LoadAssets((loadedAssets) =>
            {
                BindDependancies(loadedAssets);
                InitScene();
                InitEvents();
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
            _container.Bind<TerrainGenerationSettings>(terrainSettings);
            _container.Bind<AssetReferenceCollection>(assets);
            _container.Bind<UIDocument>(uiDocument);
            
            //Bind save controller and load save state for upcoming bindings
            _container.Bind<SaveController>();
            _container.Resolve<SaveController>().OpenSave(nextSaveToOpen);

            _container.Bind<TerrainGenerationController>();
            _container.Bind<GameUIController>();

            //Bind Generated terrain data
            var terrainData = _container.Resolve<TerrainGenerationController>().GenerateTerrain(terrainSettings);
            _container.Resolve<SaveController>().UpdateSaveStateFromTerrainData(terrainData);
            _container.Bind<TerrainGenerationData>(terrainData);
            
            //Bind inventory controllers
            _container.Bind<InventoryRelay>();
            _container.Bind<InventoryElementFactory>();
            _container.Bind<InventoryController>();
            _container.Bind<InventoryUIController>();
            
        }
        
        private void InitScene()
        {
            var generatedTerrain = _container.Resolve<TerrainGenerationData>();
            IEnumerable<ITerrainReceiver> terrainReceivers = FindObjectsOfType<MonoBehaviour>().OfType<ITerrainReceiver>();
            ApplyGeneratedTerrain(terrainReceivers, generatedTerrain, terrainSettings);
        }

        private void InitEvents() 
        {
            //TODO we still need to pass these values into something that handles these events
            _container.CollectImplementationsOfType<IUpdateReceiver>();
            _container.CollectImplementationsOfType<IDestroyReceiver>();
            _container.CollectImplementationsOfType<IPauseReceiver>();
            _container.CollectImplementationsOfType<IFocusReceiver>();
        }
        
        //TODO why does this object need to know about the next save to open, get rid of this
        public static void SetNextSaveToOpen(string saveName)
        {
            nextSaveToOpen = saveName;
        }

        private void ApplyGeneratedTerrain(
            IEnumerable<ITerrainReceiver> receivers,
            TerrainGenerationData terrainData,
            TerrainGenerationSettings terrainSettings)
        {
            foreach (ITerrainReceiver terrainReceiver in receivers)
            {
                terrainReceiver.ApplyTerrain(terrainData, terrainSettings);
            }
        }
    }
}
