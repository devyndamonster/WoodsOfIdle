using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace WoodsOfIdle
{
    public interface IPointerInfoService
    {
        public bool IsPointerOverUIObject(VisualElement rootElement);

        public Vector2 GetPointerPosition(int index = 0);
    }
}
