using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class DragAndDropElement : VisualElement
{
    private DragAndDropSlot previousSlot;
    private bool isDragging;

    public DragAndDropElement(DragAndDropSlot slot)
    {
        previousSlot = slot;

        Clear();
        AddToClassList("DragAndDropElement");

        RegisterCallback<MouseDownEvent>(OnMouseDown);
        RegisterCallback<MouseUpEvent>(OnMouseUp);
        RegisterCallback<MouseMoveEvent>(OnMouseMove);
        RegisterCallback<MouseLeaveEvent>(OnMouseLeave);
    }

    private void OnMouseDown(MouseDownEvent mouseEvent)
    {
        Debug.Log("Mouse pos: " + mouseEvent.mousePosition);
        Debug.Log("Our world pos: " + this.GetScreenPosition());

        VisualElement root = this.GetRoot();

        Debug.Log("Mouse pos within root: " + root.WorldToLocal(mouseEvent.mousePosition));
        Debug.Log("Our pos within root: " + root.WorldToLocal(this.GetScreenPosition()));

        StartDragging(mouseEvent.mousePosition);
    }

    private void OnMouseUp(MouseUpEvent mouseEvent)
    {
        StopDragging();
    }

    private void OnMouseLeave(MouseLeaveEvent mouseEvent)
    {
        Debug.Log("It Left!");

        StopDragging();
    }

    private void OnMouseMove(MouseMoveEvent mouseEvent)
    {
        if (isDragging)
        {
            Vector2 localMousePosition = this.GetRoot().WorldToLocal(mouseEvent.mousePosition);
            SetPosition(localMousePosition);
        }
    }

    private void StartDragging(Vector2 mousePosition)
    {
        if (!isDragging)
        {
            Debug.Log("Starting drag!");

            style.position = new StyleEnum<Position>(Position.Absolute);
            style.width = parent.resolvedStyle.width;
            style.height = parent.resolvedStyle.height;

            this.GetRoot().Add(this);

            SetPosition(this.GetRoot().WorldToLocal(mousePosition));

            isDragging = true;
        }
    }

    private void StopDragging()
    {
        if (isDragging)
        {
            Debug.Log("Stopping drag!");

            DragAndDropSlot destinationSlot = GetOverlappingSlot();

            if(destinationSlot is null)
            {
                destinationSlot = previousSlot;
            }

            destinationSlot.Add(this);

            style.position = new StyleEnum<Position>(Position.Relative);
            style.left = new StyleLength(StyleKeyword.Auto);
            style.top = new StyleLength(StyleKeyword.Auto);
            style.width = new StyleLength(new Length(100, LengthUnit.Percent));
            style.height = new StyleLength(new Length(100, LengthUnit.Percent));

            isDragging = false;
        }
    }

    private DragAndDropSlot GetOverlappingSlot()
    {
        foreach(DragAndDropSlot slot in this.GetRoot().Query<DragAndDropSlot>().ToList())
        {
            Vector2 screenPos = this.GetScreenPosition(VisualElementPosition.Center);
            Vector2 localPos = slot.WorldToLocal(screenPos);

            if (slot.ContainsPoint(localPos))
            {
                return slot;
            }
        }

        return null;
    }

    private void SetPosition(Vector2 pos)
    {
        Debug.Log("Previous x: " + resolvedStyle.left);
        Debug.Log("Previous y: " + resolvedStyle.top);

        Debug.Log("Setting position: " + pos);

        style.left = pos.x - resolvedStyle.width / 2;
        style.top = pos.y - resolvedStyle.height / 2;
    }
}
