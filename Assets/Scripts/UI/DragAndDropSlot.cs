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

        public new class UxmlFactory : UxmlFactory<DragAndDropSlot, UxmlTraits> { }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            UxmlStringAttributeDescription SlotIdAttr = new UxmlStringAttributeDescription { name = "Slot Id", defaultValue = "" };

            public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
            {
                get { yield break; }
            }

            public override void Init(VisualElement visualElement, IUxmlAttributes bag, CreationContext context)
            {
                base.Init(visualElement, bag, context);
                var dragAndDrop = visualElement as DragAndDropSlot;
                dragAndDrop.SlotId = SlotIdAttr.GetValueFromBag(bag, context);
                dragAndDrop.Clear();
            }
        }

        public DragAndDropElement AddItemToSlot(ItemData item, int quantity)
        {
            DragAndDropElement element = new DragAndDropElement(this);

            element.style.backgroundImage = new StyleBackground(item.ItemIcon);
            Add(element);

            return element;
        }
    }

}
