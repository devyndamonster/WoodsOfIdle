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
            SetupEvents();
            CollectDependancies();
            PopulateItemData();
        }

        private void Start()
        {
            PopulateDragAndDropSlots();
            ApplySlotStates(saveController.CurrentSaveState.InventoryInSlots);
        }

        private void SetupEvents()
        {
            FarmingNodeController.NodeHarvested += ChangeStoredItemsQuantity;
            DragAndDropElement.InventorySlotDragged += SwapInventorySlots;
        }

        private void CollectDependancies()
        {
            saveController = FindObjectOfType<SaveController>();
        }

        private void PopulateItemData()
        {
            itemData = new Dictionary<ItemType, ItemData>();
            List<ItemData> itemDataList = Resources.LoadAll<ItemData>("Items/Data").ToList();
            foreach (ItemData item in itemDataList)
            {
                itemData[item.ItemType] = item;
            }
        }

        private void PopulateDragAndDropSlots()
        {
            dragAndDropSlots = new Dictionary<string, DragAndDropSlot>();
            InventoryPanel.rootVisualElement
                .Query<DragAndDropSlot>()
                .ToList()
                .ForEach(slot => dragAndDropSlots[slot.SlotId] = slot);
        }

        private void ApplySlotStates(Dictionary<string, InventorySlotState> inventoryInSlots)
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
            Debug.Log($"Changing inventory: {itemType} changed by {quantityChange}");

            Dictionary<string, InventorySlotState> savedSlotStates = saveController.CurrentSaveState.InventoryInSlots;
            List<InventorySlotState> targetSlotStates = dragAndDropSlots.Select(slot => savedSlotStates[slot.Key]).ToList();

            List<InventoryChangeRequest> changes = inventoryService.GetInventoryChanges(targetSlotStates, itemType, quantityChange);

            Debug.Log(
                $"Saved slots count: {savedSlotStates.Count}," +
                $" target slot states count: {targetSlotStates.Count}," +
                $" drag and drop slots count: {dragAndDropSlots.Count()}," +
                $" changes count: {changes.Count()}");

            foreach (InventoryChangeRequest change in changes)
            {
                inventoryService.ApplyInventoryChange(change, targetSlotStates);
                dragAndDropSlots[change.SlotId].SetSlotState(itemData[change.ItemType], change.NewQuantity);
                Debug.Log($"Set slot state for slot: {change.SlotId} set to quantity by {change.NewQuantity}");
            }
        }

        public void SwapInventorySlots(DragAndDropSlot slotTo, DragAndDropSlot slotFrom)
        {
            Dictionary<string, InventorySlotState> savedSlotStates = saveController.CurrentSaveState.InventoryInSlots;

            if (!savedSlotStates.ContainsKey(slotTo.SlotId) || !savedSlotStates.ContainsKey(slotFrom.SlotId)) return;

            InventorySlotState slotStateTo = savedSlotStates[slotTo.SlotId];
            InventorySlotState slotStateFrom = savedSlotStates[slotFrom.SlotId];

            slotTo.SetSlotState(itemData[slotStateFrom.ItemType], slotStateFrom.Quantity);
            slotFrom.SetSlotState(itemData[slotStateTo.ItemType], slotStateTo.Quantity);
        }
    }
}
