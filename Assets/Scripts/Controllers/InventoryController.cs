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
        private List<DragAndDropSlot> dragAndDropSlots;
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
            dragAndDropSlots = InventoryPanel.rootVisualElement
                .Query<DragAndDropSlot>()
                .ToList();

            var inventoryInSlots = saveController.CurrentSaveState.InventoryInSlots;
            foreach (DragAndDropSlot slot in dragAndDropSlots)
            {
                if (inventoryInSlots.ContainsKey(slot.SlotId))
                {
                    InventorySlotState slotState = inventoryInSlots[slot.SlotId];
                    slot.SetSlotState(itemData[slotState.ItemType], slotState.Quantity);
                }
            }
        }

        public void ChangeStoredItemsQuantity(ItemType nodeType, int quantityChange)
        {
            inventoryService.ChangeStoredItemsQuantity(saveController.CurrentSaveState, nodeType, quantityChange);
        }
    }
}
