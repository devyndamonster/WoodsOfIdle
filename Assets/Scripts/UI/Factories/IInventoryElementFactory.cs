using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WoodsOfIdle
{
    public interface IInventoryElementFactory
    {
        public DragAndDropElement CreateElement(InventorySlotState state, DragAndDropSlot initialSlot);
    }
}
