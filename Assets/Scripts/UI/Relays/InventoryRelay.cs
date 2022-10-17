using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WoodsOfIdle
{
    public class InventoryRelay
    {
        private InventoryController _inventoryController;
        private InventoryUIController _inventoryUIController;
        
        public void Link(InventoryController inventoryController)
        {
            _inventoryController = inventoryController;
        }

        public void Link(InventoryUIController inventoryUIController)
        {
            _inventoryUIController = inventoryUIController;
        }
        
        public IEnumerable<InventorySlotState> RelayInventoryElementsDragged(string slotIdFrom, string slotIdTo)
        {
            return _inventoryController.SwapInventorySlots(slotIdFrom, slotIdTo);
        }
        
        public void RelayInventorySlotSettings(IEnumerable<InventorySlotData> slotData)
        {
            _inventoryController.InitializeSlotsFromData(slotData);
        }

        public IEnumerable<InventorySlotState> RequestInventorySlotStates(IEnumerable<string> slotIds)
        {
            return _inventoryController.GetSlotStates(slotIds);
        }

        public void RelayInventorySlotUpdates(IEnumerable<InventorySlotState> slotStates)
        {
            _inventoryUIController.UpdateSlotsFromStates(slotStates);
        }
    }
}
