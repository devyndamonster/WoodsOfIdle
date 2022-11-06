using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace WoodsOfIdle
{
    public class InventoryElementFactory : IInventoryElementFactory
    {
        private AssetReferenceCollection _assetCollection;
        private VisualTreeAsset _inventoryIconAsset;

        public InventoryElementFactory(AssetReferenceCollection assetCollection)
        {
            _assetCollection = assetCollection;
            _inventoryIconAsset = assetCollection.InventoryElementAsset;
        }
        
        public DragAndDropElement CreateElement(InventorySlotState state, DragAndDropSlot initialSlot, InventorySlotDragged onDrag)
        {
            var dragElement = new DragAndDropElement(initialSlot);
            _inventoryIconAsset.CloneTree(dragElement);

            var itemIcon = _assetCollection.LoadedItemData[state.ItemType].ItemIcon;
            var itemLabel = dragElement.Q<Label>("ItemLabel");
            var itemIconElement = dragElement.Q("IconElement");

            itemLabel.text = state.Quantity.ToString();
            itemIconElement.style.backgroundImage = new StyleBackground(itemIcon);
            dragElement.OnDragged += onDrag;
            
            return dragElement;
        }
    }
}
