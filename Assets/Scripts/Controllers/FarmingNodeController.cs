using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WoodsOfIdle
{
    public class FarmingNodeController
    {
        public static event ChangeStorageQuantity Harvested;
        public event Action<float> HarvestProgressChanged;
        
        public FarmingNodeData Data;
        public FarmingNodeState State;

        protected IFarmingNodeService farmingNodeService = new FarmingNodeService();

        public FarmingNodeController(IFarmingNodeService farmingNodeService, FarmingNodeData data, Vector2Int position)
        {
            this.farmingNodeService = farmingNodeService;
            
            Data = data;
            State = farmingNodeService.GetDefaultFarmingNodeState(data);
            State.Position = position;
        }

        public FarmingNodeController(IFarmingNodeService farmingNodeService, FarmingNodeData data, FarmingNodeState state)
        {
            this.farmingNodeService = farmingNodeService;
            
            Data = data;
            State = state;
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
                Harvested?.Invoke(harvestedItem, numberOfHarvests);
            }

            HarvestProgressChanged?.Invoke(farmingNodeService.CalculateHarvestProgress(State, DateTime.Now));
        }

    }
}
