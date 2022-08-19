using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace WoodsOfIdle
{
    public class DragAndDropSlot : VisualElement
    {
        public string SlotId { get; set; }
        public bool BelongsToPlayer { get; set; }
        public bool CanAutoInsert { get; set; }

        public DragAndDropElement currentElement;

        public new class UxmlFactory : UxmlFactory<DragAndDropSlot, UxmlTraits> { }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            UxmlStringAttributeDescription SlotIdAttr = new UxmlStringAttributeDescription { name = "Slot Id", defaultValue = "" };
            UxmlBoolAttributeDescription BelongsToPlayerAttr = new UxmlBoolAttributeDescription { name = "BelongsToPlayer", defaultValue = false };
            UxmlBoolAttributeDescription CanAutoInsertAttr = new UxmlBoolAttributeDescription { name = "CanAutoInsert", defaultValue = false };

            public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
            {
                get { yield break; }
            }

            public override void Init(VisualElement visualElement, IUxmlAttributes bag, CreationContext context)
            {
                base.Init(visualElement, bag, context);
                var dragAndDrop = visualElement as DragAndDropSlot;

                dragAndDrop.SlotId = SlotIdAttr.GetValueFromBag(bag, context);
                dragAndDrop.BelongsToPlayer = BelongsToPlayerAttr.GetValueFromBag(bag, context);
                dragAndDrop.CanAutoInsert = CanAutoInsertAttr.GetValueFromBag(bag, context);

                dragAndDrop.Clear();
            }
        }

        public void SetSlotState(ItemData item, int quantity)
        {
            //If element should show
            if(quantity > 0)
            {
                if (currentElement is null)
                {
                    currentElement = new DragAndDropElement(this);
                    Add(currentElement);
                }

                currentElement.style.backgroundImage = new StyleBackground(item.ItemIcon);
                currentElement.SetQuantity(quantity);
            }
            else
            {
                //Destroy the current element
                Clear();
                currentElement = null;
            }
        }
    }

}
