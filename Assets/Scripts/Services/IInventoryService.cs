using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace WoodsOfIdle
{
    public interface IInventoryService
    {
        public List<InventoryChangeRequest> GetInventoryChanges(IEnumerable<InventorySlotState> slotStates, ItemType itemType, int quantityChange);

        public void ApplyInventoryChange(InventoryChangeRequest changeRequest, IEnumerable<InventorySlotState> slotStates);

        public void SwapInventoryContents(InventorySlotState slotFrom, InventorySlotState slotTo);

        public IEnumerable<InventorySlotState> GetInventorySlotStatesFromQuantityChange(IDictionary<string, InventorySlotState> slotStates, ItemType itemType, int quantityChange);
    }
}

