using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WoodsOfIdle
{
    public class InventoryRelay
    {
        private InventoryController _inventoryController;

        public event Action<IEnumerable<InventorySlotState>> OnInventoryStateChanged;

        public InventoryRelay(InventoryController inventoryController)
        {
            _inventoryController = inventoryController;

            _inventoryController.OnInventoryStateChanged += (slotStates) => OnInventoryStateChanged?.Invoke(slotStates);
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
    }
}
