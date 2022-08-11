using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class DragAndDropElement : VisualElement
{
    private DragAndDropGrid parentGrid;
    private bool isDragging;

    public DragAndDropElement(DragAndDropGrid grid)
    {
        parentGrid = grid;
        
        Clear();
        AddToClassList("DragDropElement");

        RegisterCallback<MouseDownEvent>(OnMouseDown);
        RegisterCallback<MouseUpEvent>(OnMouseUp);
        RegisterCallback<MouseMoveEvent>(OnMouseMove);
        RegisterCallback<MouseLeaveEvent>(OnMouseLeave);
    }

    private void OnMouseDown(EventBase eventBase)
    {
        StartDragging();
    }

    private void OnMouseUp(EventBase eventBase)
    {
        StopDragging();
    }

    private void OnMouseLeave(EventBase eventBase)
    {
        Debug.Log("Left!");

        StopDragging();
    }

    private void OnMouseMove(MouseMoveEvent moveEvent)
    {
        if (isDragging)
        {
            SetPosition(parentGrid.MousePosition);
        }
    }

    private void StartDragging()
    {
        isDragging = true;
        style.position = new StyleEnum<Position>(Position.Absolute);
        style.width = parent.resolvedStyle.width;
        style.height = parent.resolvedStyle.height;

        parentGrid.Add(this);
    }

    private void StopDragging()
    {
        isDragging = false;
        style.position = new StyleEnum<Position>(Position.Relative);

        parentGrid.Children().First(child => child.name == "DragDropGridTile").Add(this);
        style.left = new StyleLength(StyleKeyword.Auto);
        style.top = new StyleLength(StyleKeyword.Auto);
        style.width = new StyleLength(new Length(100, LengthUnit.Percent));
        style.height = new StyleLength(new Length(100, LengthUnit.Percent));
    }

    private void SetPosition(Vector2 pos)
    {
        style.left = pos.x - resolvedStyle.width / 2;
        style.top = pos.y - resolvedStyle.height / 2;
    }
}
