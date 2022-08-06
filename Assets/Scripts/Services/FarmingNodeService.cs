using System;
using System.Collections;
using System.Collections.Generic;
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
    }
}

