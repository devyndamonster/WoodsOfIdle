using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WoodsOfIdle
{
    public class FarmingNodeController
    {
        public event ChangeStorageQuantity Harvested;
        public event Action<float> HarvestProgressChanged;
        
        public FarmingNodeData Data;
        public FarmingNodeState State;

        private IFarmingNodeService _farmingNodeService;

        public FarmingNodeController(IFarmingNodeService farmingNodeService, FarmingNodeData data, Vector2Int position)
        {
            _farmingNodeService = farmingNodeService;
            
            Data = data;
            State = farmingNodeService.GetDefaultFarmingNodeState(data);
            State.Position = position;
        }

        public FarmingNodeController(IFarmingNodeService farmingNodeService, FarmingNodeData data, FarmingNodeState state)
        {
            _farmingNodeService = farmingNodeService;
            
            Data = data;
            State = state;
        }
        
        public void ToggleActive()
        {
            _farmingNodeService.SetNodeActiveState(State, !State.IsActive);

            if (!State.IsActive) HarvestProgressChanged?.Invoke(0);
        }
        
        public void UpdateState()
        {
            if (!State.IsActive) return;

            int numberOfHarvests = _farmingNodeService.CalculateNumberOfHarvests(State, DateTime.Now);
            State.TimeLastHarvested = _farmingNodeService.CalculateLastHarvestTime(State, numberOfHarvests);

            if (numberOfHarvests > 0)
            {
                Dictionary<ItemType, int> harvestedItems = _farmingNodeService.GetItemsHarvested(Data, numberOfHarvests);
                
                foreach(var harvestQuantity in harvestedItems)
                {
                    Harvested?.Invoke(harvestQuantity.Key, harvestQuantity.Value);
                }
            }

            var harvestProgress = _farmingNodeService.CalculateHarvestProgress(State, DateTime.Now);
            HarvestProgressChanged?.Invoke(harvestProgress);
        }

    }
}
