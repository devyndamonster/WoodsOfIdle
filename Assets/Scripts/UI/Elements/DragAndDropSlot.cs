using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace WoodsOfIdle
{
    public class DragAndDropSlot : VisualElement
    {
        public InventorySlotData SlotData { get; set; }

        public DragAndDropElement currentElement;

        public new class UxmlFactory : UxmlFactory<DragAndDropSlot, UxmlTraits> { }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            UxmlStringAttributeDescription SlotIdAttr = new UxmlStringAttributeDescription { name = "SlotId", defaultValue = "" };
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

                dragAndDrop.SlotData = new InventorySlotData();
                dragAndDrop.SlotData.SlotId = SlotIdAttr.GetValueFromBag(bag, context);
                dragAndDrop.SlotData.BelongsToPlayer = BelongsToPlayerAttr.GetValueFromBag(bag, context);
                dragAndDrop.SlotData.CanAutoInsert = CanAutoInsertAttr.GetValueFromBag(bag, context);

                dragAndDrop.Clear();
            }
        }
    }

}
