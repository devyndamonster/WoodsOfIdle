using System;
using System.Linq;

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

        public ItemType GetItemHarvested(FarmingNodeData data)
        {
            float combinedProbability = data.HarvestableItems.Sum(x => x.ChanceToFarm);
            float randomValue = UnityEngine.Random.Range(0f, combinedProbability);
            float probabilitySum = 0f;

            foreach (var harvestableItem in data.HarvestableItems)
            {
                probabilitySum += harvestableItem.ChanceToFarm;

                if (probabilitySum >= randomValue)
                {
                    return harvestableItem.ItemType;
                }
            }

            return data.HarvestableItems.Last().ItemType;
        }
    }
}

