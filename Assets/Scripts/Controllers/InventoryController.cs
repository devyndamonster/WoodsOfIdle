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
        protected UIDocument inventoryPanel;
        protected Dictionary<string, DragAndDropSlot> dragAndDropSlots;
        protected Dictionary<ItemType, ItemData> itemData;
        
        public InventoryController(SaveController saveController, IInventoryService inventoryService, IEnumerable<ItemData> itemData, UIDocument inventoryPanel)
        {
            this.saveController = saveController;
            this.inventoryService = inventoryService;
            this.inventoryPanel = inventoryPanel;
            this.itemData = itemData.ToDictionary(item => item.ItemType);

            SetupEvents();
            PopulateDragAndDropSlots();
            ApplySlotStates(saveController.CurrentSaveState.InventoryInSlots);
        }

        private void SetupEvents()
        {
            FarmingNodeComponent.NodeHarvested += ChangeStoredItemsQuantity;
            DragAndDropElement.InventorySlotDragged += SwapInventorySlots;
        }

        private void PopulateDragAndDropSlots()
        {
            dragAndDropSlots = new Dictionary<string, DragAndDropSlot>();
            inventoryPanel.rootVisualElement
                .Query<DragAndDropSlot>()
                .ToList()
                .ForEach(slot => dragAndDropSlots[slot.SlotId] = slot);
        }

        private void ApplySlotStates(IDictionary<string, InventorySlotState> inventoryInSlots)
        {
            foreach (var slotPair in dragAndDropSlots)
            {
                if (!inventoryInSlots.ContainsKey(slotPair.Value.SlotId))
                {
                    inventoryInSlots[slotPair.Value.SlotId] = new InventorySlotState
                    {
                        SlotId = slotPair.Value.SlotId,
                        CanAutoInsert = slotPair.Value.CanAutoInsert
                    };
                }

                InventorySlotState slotState = inventoryInSlots[slotPair.Value.SlotId];
                slotPair.Value.SetSlotState(itemData[slotState.ItemType], slotState.Quantity);
            }
        }

        public void ChangeStoredItemsQuantity(ItemType itemType, int quantityChange)
        {
            IDictionary<string, InventorySlotState> savedSlotStates = saveController.CurrentSaveState.InventoryInSlots;
            List<InventorySlotState> targetSlotStates = dragAndDropSlots.Select(slot => savedSlotStates[slot.Key]).ToList();

            List<InventoryChangeRequest> changes = inventoryService.GetInventoryChanges(targetSlotStates, itemType, quantityChange);

            foreach (InventoryChangeRequest change in changes)
            {
                inventoryService.ApplyInventoryChange(change, targetSlotStates);
                dragAndDropSlots[change.SlotId].SetSlotState(itemData[change.ItemType], change.NewQuantity);
            }
        }

        public void SwapInventorySlots(DragAndDropSlot slotTo, DragAndDropSlot slotFrom)
        {
            IDictionary<string, InventorySlotState> savedSlotStates = saveController.CurrentSaveState.InventoryInSlots;

            if (!savedSlotStates.ContainsKey(slotTo.SlotId) || !savedSlotStates.ContainsKey(slotFrom.SlotId)) return;

            InventorySlotState slotStateTo = savedSlotStates[slotTo.SlotId];
            InventorySlotState slotStateFrom = savedSlotStates[slotFrom.SlotId];

            inventoryService.SwapInventoryContents(slotStateFrom, slotStateTo);

            slotTo.SetSlotState(itemData[slotStateTo.ItemType], slotStateTo.Quantity);
            slotFrom.SetSlotState(itemData[slotStateFrom.ItemType], slotStateFrom.Quantity);
        }
    }
}
