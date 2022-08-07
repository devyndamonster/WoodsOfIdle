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
        protected SaveState currentSaveState;

        private void Awake()
        {
            InitializeServices();
            CollectDependancies();
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

        public void SetSave(string levelName)
        {
            if(currentSaveState is not null)
            {
                saveService.SaveGame(currentSaveState);
            }

            currentSaveState = saveService.LoadOrCreate(levelName);
            ConnectNodesToCurrentSaveState();
        }

        private void ConnectNodesToCurrentSaveState()
        {
            foreach(FarmingNodeController node in farmingNodes)
            {
                node.ConnectToSaveState(currentSaveState);
            }
        }

        private void Update()
        {
            UpdateFarmingNodes();
            gameMenuController.UpdateDisplayFromState(currentSaveState);
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
            saveService.SaveGame(currentSaveState);
        }


    }
}
