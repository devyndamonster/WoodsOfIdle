using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(MinMaxRangeAttribute))]
public class MinMaxRangePropertyDrawer : PropertyDrawer
{
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        var container = new VisualElement();

        //MinMaxRangeAttribute minMaxRange = attribute as MinMaxRangeAttribute;

        Slider slider = new Slider(0, 1);
        slider.RegisterValueChangedCallback(OnSliderChanged);
        container.Add(slider);
        
        return container;
    }
    
    public void OnSliderChanged(ChangeEvent<float> evt)
    {
        Debug.Log("Value changed to: " + evt.newValue);
    }

}
