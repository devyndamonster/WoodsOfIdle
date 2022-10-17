using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace WoodsOfIdle
{
    public class InventoryUIController
    {
        private Dictionary<string, DragAndDropSlot> _dragAndDropSlots;
        private InventoryElementFactory _inventoryElementFactory;
        private UIDocument _uiDocument;
        private InventoryRelay _inventoryRelay;

        public InventoryUIController(InventoryElementFactory inventoryElementFactory, UIDocument uiDocument, InventoryRelay inventoryRelay)
        {
            _inventoryElementFactory = inventoryElementFactory;
            _uiDocument = uiDocument;
            _inventoryRelay = inventoryRelay;

            _inventoryRelay.Link(this);
            
            _dragAndDropSlots = _uiDocument.rootVisualElement
                .Query<DragAndDropSlot>()
                .ToList()
                .ToDictionary(slot => slot.SlotData.SlotId);

            GetInitialSlotStates();
        }

        private void GetInitialSlotStates()
        {
            _inventoryRelay.RelayInventorySlotSettings(_dragAndDropSlots.Values.Select(slot => slot.SlotData));
            var slotStates = _inventoryRelay.RequestInventorySlotStates(_dragAndDropSlots.Keys);
            UpdateSlotsFromStates(slotStates);
        }

        public void HandleElementDragged(string slotIdFrom, string slotIdTo)
        {
            var updatedSlotStates = _inventoryRelay.RelayInventoryElementsDragged(slotIdFrom, slotIdTo);
            UpdateSlotsFromStates(updatedSlotStates);
        }

        public void UpdateSlotsFromStates(IEnumerable<InventorySlotState> slotStates)
        {
            foreach(InventorySlotState slotState in slotStates)
            {
                SetSlotState(_dragAndDropSlots[slotState.SlotId], slotState);
            }
        }

        private void SetSlotState(DragAndDropSlot slot, InventorySlotState slotState)
        {
            slot.Clear();

            if (slotState.Quantity > 0)
            {
                VisualElement inventoryElement = _inventoryElementFactory.CreateElement(slotState, slot);
                slot.Add(inventoryElement);
            }
        }

    }
}
