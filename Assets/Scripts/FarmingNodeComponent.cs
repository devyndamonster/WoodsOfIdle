using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WoodsOfIdle
{
    public class FarmingNodeComponent : MonoBehaviour
    {
        public Button activateButton;

        public FarmingNodeState state;

        private GameController gameController;

        private void Awake()
        {
            gameController = FindObjectOfType<GameController>();
        }

        private void Start()
        {
            ConnectToSaveState();
        }

        private void ConnectToSaveState()
        {
            if (gameController.currentSaveState.FarmingNodes.ContainsKey(state.NodeId))
            {
                state = gameController.currentSaveState.FarmingNodes[state.NodeId];
            }
            else
            {
                gameController.currentSaveState.FarmingNodes[state.NodeId] = state;
            }
        }

        private void SetActive(bool isActive)
        {
            state.IsActive = isActive;
        }
    }
}
