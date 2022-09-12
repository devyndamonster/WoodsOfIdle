using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace WoodsOfIdle
{
    public class PointerInfoService : IPointerInfoService
    {
        public Vector2 GetPointerPosition(int index = 0)
        {
#if UNITY_EDITOR
            return Mouse.current.position.ReadValue();
#elif UNITY_ANDROID || UNITY_IOS
            return Touchscreen.current.touches[index].position.ReadValue();
#endif
        }

        public bool IsPointerOverUIObject(VisualElement rootElement)
        {
            throw new System.NotImplementedException();
        }
    }
}
