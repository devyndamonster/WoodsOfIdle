using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DragAndDropSlot : VisualElement
{
    public new class UxmlFactory : UxmlFactory<DragAndDropGrid, UxmlTraits> { }

    public new class UxmlTraits : VisualElement.UxmlTraits
    {
        public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
        {
            get { yield break; }
        }

        public override void Init(VisualElement visualElement, IUxmlAttributes bag, CreationContext context)
        {
            base.Init(visualElement, bag, context);
            var dragAndDrop = visualElement as DragAndDropSlot;
            dragAndDrop.Clear();
        }
    }
}
