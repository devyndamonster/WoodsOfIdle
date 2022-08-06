using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WoodsOfIdle
{
    public class FarmingNodeController : MonoBehaviour
    {
        public Button activateButton;

        public FarmingNodeState state = new FarmingNodeState();

        protected IFarmingNodeService farmingNodeService = new FarmingNodeService();

        public void ConnectToSaveState(SaveState saveState)
        {
            if (saveState.FarmingNodes.ContainsKey(state.NodeId))
            {
                state = saveState.FarmingNodes[state.NodeId];
            }
            else
            {
                saveState.FarmingNodes[state.NodeId] = state;
            }
        }

        public void ToggleActive()
        {
            SetActive(!state.IsActive);
        }

        public void UpdateState()
        {
            int numberOfHarvests = farmingNodeService.CalculateNumberOfHarvests(state, DateTime.Now);
            state.TimeLastHarvested = farmingNodeService.CalculateLastHarvestTime(state, numberOfHarvests);

        }

        private void SetActive(bool isActive)
        {
            state.IsActive = isActive;
        }
    }
}
