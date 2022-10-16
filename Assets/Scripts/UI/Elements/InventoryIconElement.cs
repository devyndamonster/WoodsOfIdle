using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace WoodsOfIdle
{
    public class InventoryIconElement : VisualElement
    {
        private VisualTreeAsset _inventoryIconAsset;
        
        public InventoryIconElement(VisualTreeAsset inventoryIconAsset, Sprite icon, int quantity)
        {
            _inventoryIconAsset = inventoryIconAsset;
            Add(_inventoryIconAsset.CloneTree());
        }
    }
}
