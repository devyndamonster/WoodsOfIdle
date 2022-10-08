using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WoodsOfIdle
{
    public interface IInventoryReceiver
    {
        public void ApplyInventoryState(IEnumerable<InventorySlotState> inventoryStates);
    }
}
