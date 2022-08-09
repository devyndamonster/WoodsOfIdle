using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;



//This was a nice resource: https://forum.unity.com/threads/ui-builder-and-custom-elements.785129/

public class DragAndDropGrid : VisualElement
{
    internal Clickable m_Clickable;

    public new class UxmlFactory : UxmlFactory<DragAndDropGrid, UxmlTraits> { }

    public new class UxmlTraits : VisualElement.UxmlTraits
    {
        UxmlIntAttributeDescription m_Int = new UxmlIntAttributeDescription { name = "int-attr", defaultValue = 2 };

        public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
        {
            get { yield break; }
        }

        public override void Init(VisualElement visualElement, IUxmlAttributes bag, CreationContext context)
        {
            base.Init(visualElement, bag, context);
            var dragAndDrop = visualElement as DragAndDropGrid;

            dragAndDrop.Clear();

            dragAndDrop.intAttr = m_Int.GetValueFromBag(bag, context);
        }
    }

    public DragAndDropGrid()
    {
        RegisterCallback<MouseMoveEvent>(OnClickEvent);
    }

    private void OnClickEvent(EventBase eventBase)
    {
        style.backgroundColor = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
    }

    public int intAttr { get; set; }

}
