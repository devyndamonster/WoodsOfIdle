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

        public SaveState currentSaveState;
        public List<FarmingNodeController> farmingNodes = new List<FarmingNodeController>();
        public Text testText;

        private void Awake()
        {
            InitializeNodes();
            InitializeServices();
        }

        private void Start()
        {
            SetSave("test");
        }

        private void InitializeServices()
        {
            saveService = new SaveService();
        }

        private void InitializeNodes()
        {
            farmingNodes = FindObjectsOfType<FarmingNodeController>().ToList();
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
        }

        private void UpdateFarmingNodes()
        {
            foreach(FarmingNodeController farmingNode in farmingNodes)
            {
                farmingNode.UpdateState();
            }

            testText.text = currentSaveState.StoredItems[NodeType.Wood].ToString();
        }


        private void OnDestroy()
        {
            saveService.SaveGame(currentSaveState);
        }


    }
}
