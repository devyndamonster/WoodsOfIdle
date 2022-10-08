using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace WoodsOfIdle
{
    public class InventoryController
    {
        protected IInventoryService inventoryService;
        protected SaveController saveController;
        protected Dictionary<ItemType, ItemData> itemData;
        
        public InventoryController(SaveController saveController, IInventoryService inventoryService, Dictionary<ItemType, ItemData> itemData)
        {
            this.saveController = saveController;
            this.inventoryService = inventoryService;
            this.itemData = itemData;
            
        }
        
        public IEnumerable<InventorySlotState> ChangeStoredItemsQuantity(ItemType itemType, int quantityChange)
        {
            IDictionary<string, InventorySlotState> savedSlotStates = saveController.CurrentSaveState.InventoryInSlots;
            List<InventoryChangeRequest> changes = inventoryService.GetInventoryChanges(savedSlotStates.Values, itemType, quantityChange);
            List<InventorySlotState> updatedStates = new List<InventorySlotState>();

            foreach (InventoryChangeRequest change in changes)
            {
                inventoryService.ApplyInventoryChange(change, savedSlotStates.Values);
                updatedStates.Add(savedSlotStates[change.SlotId]);
            }

            return updatedStates;
        }
        
        public IEnumerable<InventorySlotState> SwapInventorySlots(string slotIdTo, string slotIdFrom)
        {
            IDictionary<string, InventorySlotState> savedSlotStates = saveController.CurrentSaveState.InventoryInSlots;

            if (!savedSlotStates.ContainsKey(slotIdTo) || !savedSlotStates.ContainsKey(slotIdFrom)) return Enumerable.Empty<InventorySlotState>();

            InventorySlotState slotStateTo = savedSlotStates[slotIdTo];
            InventorySlotState slotStateFrom = savedSlotStates[slotIdFrom];

            inventoryService.SwapInventoryContents(slotStateFrom, slotStateTo);

            return new InventorySlotState[] { slotStateTo, slotStateFrom };
        }
    }
}
