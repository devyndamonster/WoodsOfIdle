using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WoodsOfIdle
{
    public class IFarmingNodeService
    {
        public int GetNumberOfHarvests(FarmingNodeState state, DateTime currentTime)
        {
            double timePassed = (state.TimeLastHarvested - currentTime).TotalSeconds;

            return (int)(timePassed / state.TimeToHarvest);
        }
    }
}

