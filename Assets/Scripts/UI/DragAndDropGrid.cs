using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;



//This was a nice resource: https://forum.unity.com/threads/ui-builder-and-custom-elements.785129/

public class DragAndDropGrid : VisualElement
{
    public int Columns { get; set; }
    public int Rows { get; set; }


    public new class UxmlFactory : UxmlFactory<DragAndDropGrid, UxmlTraits> { }

    public new class UxmlTraits : VisualElement.UxmlTraits
    {
        UxmlIntAttributeDescription ColumnsAttr = new UxmlIntAttributeDescription { name = "Columns", defaultValue = 4 };
        UxmlIntAttributeDescription RowsAttr = new UxmlIntAttributeDescription { name = "Rows", defaultValue = 4 };

        public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
        {
            get { yield break; }
        }

        public override void Init(VisualElement visualElement, IUxmlAttributes bag, CreationContext context)
        {
            base.Init(visualElement, bag, context);
            var dragAndDrop = visualElement as DragAndDropGrid;

            dragAndDrop.Columns = ColumnsAttr.GetValueFromBag(bag, context);
            dragAndDrop.Rows = RowsAttr.GetValueFromBag(bag, context);

            dragAndDrop.Clear();

            for(int index = 0; index < dragAndDrop.Columns * dragAndDrop.Rows; index++)
            {
                VisualElement element = new VisualElement();
                element.name = "DragDropGridTile";
                element.AddToClassList("DragDropGridTile");
                dragAndDrop.Add(element);
            }

            dragAndDrop.RefreshGridSizes();
        }
    }

    public DragAndDropGrid()
    {
        RefreshGridSizes();
        RegisterCallback<GeometryChangedEvent>(OnGeometryUpdate);
    }

    private void OnGeometryUpdate(EventBase eventBase)
    {
        RefreshGridSizes();
    }

    public void RefreshGridSizes()
    {
        foreach (VisualElement element in Children().Where(child => child.name == "DragDropGridTile"))
        {
            element.style.width = resolvedStyle.width / Columns;
            element.style.height = element.style.width;
        }
    }

    /*
    private StyleLength GetTileWidth()
    {
        Length length = new Length()
        {
            unit = LengthUnit.Pixel,
            value = 
        }


        return new StyleLength(new Length { value });
    }

    private StyleLength GetTileHeight()
    {

    }
    */
    

}
