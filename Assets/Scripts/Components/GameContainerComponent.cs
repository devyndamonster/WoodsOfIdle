using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WoodsOfIdle
{
    public class GameContainerComponent : MonoBehaviour
    {
        protected DependancyContainer _container = new DependancyContainer();
        protected static string nextSaveToOpen;

        private void Start()
        {
            BindDependancies();
            InitScene();
        }

        private void BindDependancies()
        {
            _container.Bind<AssetReferenceCollection>(null);

            _container.Bind<ISaveService, SaveService>();
            _container.Bind<IAddressableLoaderService, AddressableLoaderService>();
            _container.Bind<IInventoryService, InventoryService>();
            _container.Bind<IFarmingNodeService, FarmingNodeService>();
            _container.Bind<ITerrainService, TerrainService>();

            _container.Bind<SaveController>();
            _container.Bind<TerrainGenerationController>();

            //Still need to create UI and game controller
        }

        private void InitScene()
        {
            // Use mediator to handle all game setup events

            // Need to open new save
            // Need to generate terrain and apply it to level state
            // Need to pass initial state to UI
        }
    }
}
