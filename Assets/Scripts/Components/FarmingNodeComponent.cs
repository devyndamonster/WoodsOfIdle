using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WoodsOfIdle
{
    public class FarmingNodeComponent : MonoBehaviour
    {
        public FarmingNodeData Data;
        public FarmingNodeState State = new FarmingNodeState();

        public static event ChangeStorageQuantity NodeHarvested;

        protected IFarmingNodeService farmingNodeService = new FarmingNodeService();

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
            farmingNodeService.SetNodeActiveState(State, !State.IsActive);
        }

        public void UpdateState()
        {
            int numberOfHarvests = farmingNodeService.CalculateNumberOfHarvests(State, DateTime.Now);
            State.TimeLastHarvested = farmingNodeService.CalculateLastHarvestTime(State, numberOfHarvests);
            
            if (numberOfHarvests > 0)
            {
                ItemType harvestedItem = farmingNodeService.GetItemHarvested(Data);
                NodeHarvested(harvestedItem, numberOfHarvests);
            }
            
        }
    }
}
