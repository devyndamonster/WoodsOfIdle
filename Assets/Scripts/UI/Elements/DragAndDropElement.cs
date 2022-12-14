using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;



namespace WoodsOfIdle
{
    public class DragAndDropElement : VisualElement
    {
        public event InventorySlotDragged OnDragged;

        private DragAndDropSlot previousSlot;
        private bool isDragging;

        public DragAndDropElement(DragAndDropSlot slot)
        {
            AddToClassList("DragAndDropElementContainer");

            previousSlot = slot;
            
            RegisterCallback<MouseDownEvent>(OnMouseDown);
            RegisterCallback<MouseUpEvent>(OnMouseUp);
            RegisterCallback<MouseMoveEvent>(OnMouseMove);
            RegisterCallback<MouseLeaveEvent>(OnMouseLeave);
        }
        
        private void OnMouseDown(MouseDownEvent mouseEvent)
        {
            StartDragging(mouseEvent.mousePosition);
        }

        private void OnMouseUp(MouseUpEvent mouseEvent)
        {
            StopDragging();
        }

        private void OnMouseLeave(MouseLeaveEvent mouseEvent)
        {
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
                SetStyleDragging();
                VisualElement root = this.GetRoot();
                root.Add(this);
                SetPosition(root.WorldToLocal(mousePosition));
                isDragging = true;
            }
        }

        private void StopDragging()
        {
            if (isDragging)
            {
                DragAndDropSlot destinationSlot = GetOverlappingSlot();
                if (destinationSlot is null)
                {
                    destinationSlot = previousSlot;
                }

                previousSlot.Add(this);
                SetStyleNotDragging();
                isDragging = false;

                if (!destinationSlot.Equals(previousSlot))
                {
                    OnDragged?.Invoke(destinationSlot.SlotData.SlotId, previousSlot.SlotData.SlotId);
                }
            }
        }

        private DragAndDropSlot GetOverlappingSlot()
        {
            foreach (DragAndDropSlot slot in this.GetRoot().Query<DragAndDropSlot>().ToList())
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

        private void SetStyleDragging()
        {
            style.position = new StyleEnum<Position>(Position.Absolute);
            style.width = parent.resolvedStyle.width;
            style.height = parent.resolvedStyle.height;
        }

        private void SetStyleNotDragging()
        {
            style.position = new StyleEnum<Position>(Position.Relative);
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
}

