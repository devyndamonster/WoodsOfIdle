using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace WoodsOfIdle
{
    public class GameController : MonoBehaviour
    {
        protected ISaveService saveService;

        protected List<FarmingNodeController> farmingNodes;
        protected GameMenuController gameMenuController;

        public SaveState CurrentSaveState { get; protected set; }

        private void Awake()
        {
            InitializeServices();
            CollectDependancies();
            SetupEvents();
        }

        private void Start()
        {
            SetSave("test");
        }

        private void InitializeServices()
        {
            saveService = new SaveService();
        }

        private void CollectDependancies()
        {
            farmingNodes = FindObjectsOfType<FarmingNodeController>().ToList();
            gameMenuController = FindObjectOfType<GameMenuController>();
        }

        private void SetupEvents()
        {
            foreach(FarmingNodeController farmingNode in farmingNodes)
            {
                farmingNode.NodeHarvested += ChangeStoredItemsQuantity;
            }
        }

        private void ConnectNodesToCurrentSaveState()
        {
            foreach(FarmingNodeController node in farmingNodes)
            {
                node.ConnectToSaveState(CurrentSaveState);
            }
        }

        private void Update()
        {
            UpdateFarmingNodes();
            gameMenuController.UpdateDisplayFromState(CurrentSaveState);
        }

        private void UpdateFarmingNodes()
        {
            foreach(FarmingNodeController farmingNode in farmingNodes)
            {
                farmingNode.UpdateState();
            }
        }

        private void OnDestroy()
        {
            saveService.SaveGame(CurrentSaveState);
        }

        private void ChangeStoredItemsQuantity(NodeType nodeType, int quantityChange)
        {
            CurrentSaveState.StoredItems[nodeType] += quantityChange;
        }

        public void SetSave(string levelName)
        {
            if (CurrentSaveState is not null)
            {
                saveService.SaveGame(CurrentSaveState);
            }

            CurrentSaveState = saveService.LoadOrCreate(levelName);
            ConnectNodesToCurrentSaveState();
        }

    }
}
