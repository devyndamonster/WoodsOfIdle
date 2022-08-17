using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace WoodsOfIdle
{
    public interface IInventoryService
    {
        public List<InventoryChangeRequest> GetInventoryChanges(List<InventorySlotState> slotStates, ItemType itemType, int quantityChange);

        public void ApplyInventoryChange(InventoryChangeRequest changeRequest, List<InventorySlotState> slotStates);
    }
}

