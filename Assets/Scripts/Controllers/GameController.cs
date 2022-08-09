using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace WoodsOfIdle
{
    public class GameController : MonoBehaviour
    {
        protected List<FarmingNodeController> farmingNodes;
        protected GameMenuController gameMenuController;
        protected SaveController saveController;

        private void Awake()
        {
            CollectDependancies();
            SetupEvents();
        }

        private void Start()
        {
            saveController.OpenSave();
        }

        private void CollectDependancies()
        {
            farmingNodes = FindObjectsOfType<FarmingNodeController>().ToList();
            gameMenuController = FindObjectOfType<GameMenuController>();
            saveController = FindObjectOfType<SaveController>();
        }

        private void SetupEvents()
        {
            foreach(FarmingNodeController farmingNode in farmingNodes)
            {
                farmingNode.NodeHarvested += ChangeStoredItemsQuantity;
            }
        }

        private void Update()
        {
            UpdateFarmingNodes();
            gameMenuController.UpdateDisplayFromState(saveController.CurrentSaveState);
        }

        private void UpdateFarmingNodes()
        {
            foreach(FarmingNodeController farmingNode in farmingNodes)
            {
                farmingNode.UpdateState();
            }
        }

        private void ChangeStoredItemsQuantity(NodeType nodeType, int quantityChange)
        {
            saveController.CurrentSaveState.StoredItems[nodeType] += quantityChange;
        }

        

    }
}
