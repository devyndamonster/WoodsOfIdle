using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace WoodsOfIdle
{
    public interface IPointerInfoService
    {
        public bool IsScreenPositionOverUIObject(VisualElement rootElement, Vector2 screenPosition);

        public VisualElement GetVisualElementAtScreenPosition(VisualElement rootElement, Vector2 screenPosition);

        public Vector2 GetPointerPosition(int index = 0);
    }
}
