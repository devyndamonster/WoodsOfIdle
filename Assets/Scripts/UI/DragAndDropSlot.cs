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

        public new class UxmlFactory : UxmlFactory<DragAndDropSlot, UxmlTraits> { }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            UxmlStringAttributeDescription SlotIdAttr = new UxmlStringAttributeDescription { name = "Slot Id", defaultValue = "" };
            UxmlBoolAttributeDescription BelongsToPlayerAttr = new UxmlBoolAttributeDescription { name = "Belongs To Player", defaultValue = false };

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

                dragAndDrop.Clear();
            }
        }

        public void SetSlotState(ItemData item, int quantity)
        {
            if(quantity <= 0)
            {
                Clear();
            }

            if(quantity > 0)
            {
                DragAndDropElement element = this.Q<DragAndDropElement>();
                if (element is null)
                {
                    element = new DragAndDropElement(this);
                }

                element.style.backgroundImage = new StyleBackground(item.ItemIcon);
                Add(element);
            }
        }
    }

}
