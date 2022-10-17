using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace WoodsOfIdle
{
    public class InventoryElementFactory
    {
        private AssetReferenceCollection _assetCollection;
        private VisualTreeAsset _inventoryIconAsset;

        public InventoryElementFactory(AssetReferenceCollection assetCollection)
        {
            _assetCollection = assetCollection;
            _inventoryIconAsset = assetCollection.InventoryElementAsset;
        }

        
        public DragAndDropElement CreateElement(InventorySlotState state, DragAndDropSlot initialSlot)
        {
            var itemIcon = _assetCollection.LoadedItemData[state.ItemType].ItemIcon;
            var dragElement = new DragAndDropElement(initialSlot);
            var inventoryIconElement = new InventoryIconElement(_inventoryIconAsset, itemIcon, state.Quantity);

            dragElement.Add(inventoryIconElement);
            
            return null;
        }
    }
}
