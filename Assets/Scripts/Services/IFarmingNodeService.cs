using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WoodsOfIdle
{
    public interface IFarmingNodeService
    {
        public int CalculateNumberOfHarvests(FarmingNodeState state, DateTime currentTime);

        public DateTime CalculateLastHarvestTime(FarmingNodeState state, int numberOfHarvests);

        public void SetNodeActiveState(FarmingNodeState state, bool isActive);

        public Dictionary<ItemType, int> GetItemsHarvested(FarmingNodeData data, int quantityHarvested);

        public float CalculateHarvestProgress(FarmingNodeState state, DateTime currentTime);

        public FarmingNodeState GetDefaultFarmingNodeState(FarmingNodeData data);
        
    }
}

