using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace WoodsOfIdle
{
    public class InventoryController
    {
        private IInventoryService _inventoryService;
        private SaveController _saveController;
        private InventoryRelay _inventoryRelay;

        public InventoryController(
            SaveController saveController,
            IInventoryService inventoryService,
            InventoryRelay inventoryRelay)
        {
            _saveController = saveController;
            _inventoryService = inventoryService;
            _inventoryRelay = inventoryRelay;

            _inventoryRelay.Link(this);
        }

        public void OnItemQuantityChanged(ItemType itemType, int quantityChange)
        {
            _inventoryRelay.RelayInventorySlotUpdates(ChangeStoredItemsQuantity(itemType, quantityChange));
        }

        public void InitializeSlotsFromData(IEnumerable<InventorySlotData> slotData)
        {
            var savedSlots = _saveController.CurrentSaveState.InventoryInSlots;

            foreach (var slot in slotData)
            {
                if (!savedSlots.ContainsKey(slot.SlotId))
                {
                    savedSlots[slot.SlotId] = new InventorySlotState
                    {
                        SlotId = slot.SlotId,
                        CanAutoInsert = slot.CanAutoInsert
                    };
                }
            }
        }

        public IEnumerable<InventorySlotState> GetSlotStates(IEnumerable<string> slotIds)
        {
            return slotIds.Select(slotId => _saveController.CurrentSaveState.InventoryInSlots[slotId]);
        }
        
        public IEnumerable<InventorySlotState> ChangeStoredItemsQuantity(ItemType itemType, int quantityChange)
        {
            var savedSlotStates = _saveController.CurrentSaveState.InventoryInSlots;
            var updatedStates = _inventoryService.GetInventorySlotStatesFromQuantityChange(savedSlotStates, itemType, quantityChange);
            return updatedStates;
        }
        
        public IEnumerable<InventorySlotState> SwapInventorySlots(string slotIdTo, string slotIdFrom)
        {
            var savedSlotStates = _saveController.CurrentSaveState.InventoryInSlots;

            if (!savedSlotStates.ContainsKey(slotIdTo) || !savedSlotStates.ContainsKey(slotIdFrom)) return Enumerable.Empty<InventorySlotState>();
            
            var slotStateTo = savedSlotStates[slotIdTo];
            var slotStateFrom = savedSlotStates[slotIdFrom];

            _inventoryService.SwapInventoryContents(slotStateFrom, slotStateTo);

            return new InventorySlotState[] { slotStateTo, slotStateFrom };
        }
    }
}
