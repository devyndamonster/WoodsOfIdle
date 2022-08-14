using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace WoodsOfIdle
{
    public interface IInventoryService
    {
        public void ChangeStoredItemsQuantity(SaveState saveState, ItemType type, int quantity);
    }
}

