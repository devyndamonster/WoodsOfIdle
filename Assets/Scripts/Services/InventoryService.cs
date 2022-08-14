using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WoodsOfIdle
{
    public class InventoryService : IInventoryService
    {
        public void ChangeStoredItemsQuantity(SaveState saveState, ItemType type, int quantity)
        {
            if(quantity > 0)
            {
                AddItemsToInventory(saveState, type, quantity);
            }

            else if(quantity < 0)
            {
                RemoveItemsFromInventory(saveState, type, quantity);
            }
        }

        private void AddItemsToInventory(SaveState saveState, ItemType type, int quantity)
        {
            
        }

        private void RemoveItemsFromInventory(SaveState saveState, ItemType type, int quantity)
        {

        }
    }
}

