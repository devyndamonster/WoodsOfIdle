using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace WoodsOfIdle
{
    public class InventoryController : MonoBehaviour
    {
        public UIDocument InventoryPanel;

        private SaveController saveController;
        private Dictionary<string, DragAndDropSlot> dragAndDropSlots;
        private Dictionary<ItemType, ItemData> itemData;

        private IInventoryService inventoryService = new InventoryService();

        private void Awake()
        {
            FarmingNodeController.NodeHarvested += ChangeStoredItemsQuantity;

            saveController = FindObjectOfType<SaveController>();
            List<ItemData> itemDataList = Resources.LoadAll<ItemData>("Items/Data").ToList();
            foreach(ItemData item in itemDataList)
            {
                itemData[item.ItemType] = item;
            }
        }

        private void Start()
        {
            InventoryPanel.rootVisualElement
                .Query<DragAndDropSlot>()
                .ToList()
                .ForEach(slot => dragAndDropSlots[slot.SlotId] = slot);

            var inventoryInSlots = saveController.CurrentSaveState.InventoryInSlots;
            foreach (var slotPair in dragAndDropSlots)
            {
                if (inventoryInSlots.ContainsKey(slotPair.Value.SlotId))
                {
                    InventorySlotState slotState = inventoryInSlots[slotPair.Value.SlotId];
                    slotPair.Value.SetSlotState(itemData[slotState.ItemType], slotState.Quantity);
                }
            }
        }

        public void ChangeStoredItemsQuantity(ItemType itemType, int quantityChange)
        {
            Dictionary<string, InventorySlotState> savedSlotStates = saveController.CurrentSaveState.InventoryInSlots;
            List<InventorySlotState> targetSlotStates = dragAndDropSlots.Select(slot => savedSlotStates[slot.Key]).ToList();

            List<InventoryChangeRequest> changes = inventoryService.GetInventoryChanges(targetSlotStates, itemType, quantityChange);
            foreach(InventoryChangeRequest change in changes)
            {
                inventoryService.ApplyInventoryChange(change, targetSlotStates);
                dragAndDropSlots[change.SlotId].SetSlotState(itemData[change.ItemType], change.NewQuantity);
            }
        }
    }
}
