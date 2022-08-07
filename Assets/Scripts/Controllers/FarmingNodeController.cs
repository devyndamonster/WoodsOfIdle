using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WoodsOfIdle
{
    public class FarmingNodeController : MonoBehaviour
    {
        public FarmingNodeState State = new FarmingNodeState();
        public event ChangeStorageQuantity NodeHarvested;

        protected IFarmingNodeService farmingNodeService = new FarmingNodeService();

        [SerializeField]
        private Button activateButton;

        public void ConnectToSaveState(SaveState saveState)
        {
            if (saveState.FarmingNodes.ContainsKey(State.NodeId))
            {
                State = saveState.FarmingNodes[State.NodeId];
            }
            else
            {
                saveState.FarmingNodes[State.NodeId] = State;
            }
        }

        public void ToggleActive()
        {
            SetActive(!State.IsActive);
        }

        public void UpdateState()
        {
            int numberOfHarvests = farmingNodeService.CalculateNumberOfHarvests(State, DateTime.Now);
            State.TimeLastHarvested = farmingNodeService.CalculateLastHarvestTime(State, numberOfHarvests);

            if (numberOfHarvests > 0) NodeHarvested(State.NodeType, numberOfHarvests);
        }

        private void SetActive(bool isActive)
        {
            State.IsActive = isActive;
        }
    }
}
