using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public static class VisualElementExtensions
{
    public static void RemoveAllChildren(this VisualElement visualElement)
    {
        for(int childIndex = visualElement.childCount - 1; childIndex >= 0; childIndex--)
        {
            visualElement.RemoveAt(childIndex);
        }
    }
}
