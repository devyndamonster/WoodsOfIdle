using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WoodsOfIdle
{
    public class FarmingNodeComponent : MonoBehaviour, IClickable
    {
        public FarmingNodeData Data;
        public FarmingNodeState State = new FarmingNodeState();

        public static event ChangeStorageQuantity NodeHarvested;
        public static event Action<FarmingNodeComponent> NodeClicked;
        public event Action<float> HarvestProgressChanged;

        protected IFarmingNodeService farmingNodeService = new FarmingNodeService();

        public void ConnectToSaveState(SaveState saveState)
        {
            if (saveState.FarmingNodes.ContainsKey(State.Position))
            {
                State = saveState.FarmingNodes[State.Position];
            }
            else
            {
                saveState.FarmingNodes[State.Position] = State;
            }
        }

        public void ToggleActive()
        {
            farmingNodeService.SetNodeActiveState(State, !State.IsActive);
        }



        //TODO: Should the process of updating state be totally removed from the component itself?
        public void UpdateState()
        {
            int numberOfHarvests = farmingNodeService.CalculateNumberOfHarvests(State, DateTime.Now);
            State.TimeLastHarvested = farmingNodeService.CalculateLastHarvestTime(State, numberOfHarvests);
            
            if (numberOfHarvests > 0)
            {
                ItemType harvestedItem = farmingNodeService.GetItemHarvested(Data);
                NodeHarvested(harvestedItem, numberOfHarvests);
            }

            HarvestProgressChanged?.Invoke(farmingNodeService.CalculateHarvestProgress(State, DateTime.Now));
        }

        public void Click()
        {
            NodeClicked(this);
        }
    }
}
