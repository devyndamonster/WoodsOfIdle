using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WoodsOfIdle
{
    public class InventoryService : IInventoryService
    {
        public List<InventoryChangeRequest> GetInventoryChanges(List<InventorySlotState> slotStates, ItemType itemType, int quantityChange)
        {
            return null;
        }

        public void ApplyInventoryChange(InventoryChangeRequest changeRequest, List<InventorySlotState> slotStates)
        {

        }
    }
}

