using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public enum VisualElementPosition
{
    TopRight,
    TopLeft,
    BottomLeft,
    BottomRight,
    Center
}


public static class VisualElementExtensions
{
    public static VisualElement GetRoot(this VisualElement visualElement)
    {
        VisualElement rootElement = visualElement.parent;

        while(rootElement is not null && rootElement.parent is not null && rootElement.name != "ScreenContainer")
        {
            rootElement = rootElement.parent;
        }

        return rootElement;
    }

    public static Vector2 GetScreenPosition(this VisualElement visualElement, VisualElementPosition positionType = VisualElementPosition.Center)
    {
        Vector2 positionOnElement = Vector2.zero;

        if(positionType == VisualElementPosition.TopRight || positionType == VisualElementPosition.BottomRight)
        {
            positionOnElement.x = visualElement.resolvedStyle.width;
        }

        if (positionType == VisualElementPosition.BottomRight || positionType == VisualElementPosition.BottomLeft)
        {
            positionOnElement.y = visualElement.resolvedStyle.height;
        }

        if(positionType == VisualElementPosition.Center)
        {
            positionOnElement.x = visualElement.resolvedStyle.width / 2;
            positionOnElement.y = visualElement.resolvedStyle.height / 2;
        }

        return visualElement.LocalToWorld(positionOnElement);
    }

    
    public static bool IsWithinVisualElement(this VisualElement thisElement, VisualElement otherElement)
    {
        Vector2 minPosition = GetScreenPosition(otherElement, VisualElementPosition.TopLeft);
        Vector2 maxPosition = GetScreenPosition(otherElement, VisualElementPosition.BottomRight);
        Vector2 centerPosition = GetScreenPosition(thisElement, VisualElementPosition.Center);

        return centerPosition.x >= minPosition.x 
            && centerPosition.x <= maxPosition.x 
            && centerPosition.y >= minPosition.y 
            && centerPosition.y <= maxPosition.y;
    }
    

}
