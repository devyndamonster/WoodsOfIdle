using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WoodsOfIdle
{
    public class FarmingNodeService : IFarmingNodeService
    {
        public int CalculateNumberOfHarvests(FarmingNodeState state, DateTime currentTime)
        {
            if (!state.IsActive) return 0;

            double timePassed = (currentTime - state.TimeLastHarvested).TotalSeconds;

            return (int)(timePassed / state.TimeToHarvest);
        }

        public DateTime CalculateLastHarvestTime(FarmingNodeState state, int numberOfHarvests)
        {
            return state.TimeLastHarvested.AddSeconds(state.TimeToHarvest * numberOfHarvests);
        }

        public void SetNodeActiveState(FarmingNodeState state, bool isActive)
        {
            if(isActive && !state.IsActive)
            {
                state.TimeLastHarvested = DateTime.Now;
            }

            state.IsActive = isActive;
        }

        public Dictionary<ItemType, int> GetItemsHarvested(FarmingNodeData data, int quantityHarvested)
        {
            Dictionary<ItemType, int> harvestedItems = new Dictionary<ItemType, int>();
            
            for(int harvestIndex = 0; harvestIndex < quantityHarvested; harvestIndex++)
            {
                ItemType selectedItem = data.HarvestableItems.GetRandomFromWeight(item => item.ChanceToFarm).ItemType;
                
                if (harvestedItems.ContainsKey(selectedItem))
                {
                    harvestedItems[selectedItem] += 1;
                }
                else
                {
                    harvestedItems[selectedItem] = 1;
                }
            }
            
            return harvestedItems;
        }

        public float CalculateHarvestProgress(FarmingNodeState state, DateTime currentTime)
        {
            return Mathf.Clamp01((float)((currentTime - state.TimeLastHarvested).TotalSeconds / state.TimeToHarvest));
        }

        public FarmingNodeState GetDefaultFarmingNodeState(FarmingNodeData data)
        {
            return new FarmingNodeState
            {
                IsActive = false,
                TimeLastHarvested = DateTime.Now,
                TimeToHarvest = data.DefaultTimeToHarvest,
                NodeType = data.NodeType
            };
        }
    }
}

