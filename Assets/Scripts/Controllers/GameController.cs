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
        protected SaveController saveController;
        protected InventoryController inventoryController;

        private void Awake()
        {
            CollectDependancies();
        }

        private void Start()
        {
            saveController.OpenSave();
        }

        private void CollectDependancies()
        {
            farmingNodes = FindObjectsOfType<FarmingNodeController>().ToList();
            saveController = FindObjectOfType<SaveController>();
            inventoryController = FindObjectOfType<InventoryController>();
        }

        private void Update()
        {
            UpdateFarmingNodes();
        }

        private void UpdateFarmingNodes()
        {
            foreach(FarmingNodeController farmingNode in farmingNodes)
            {
                farmingNode.UpdateState();
            }
        }
    }
}
