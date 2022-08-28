using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MinMaxRangeAttribute : PropertyAttribute
{
    public float Min { get; set; }
    public float Max { get; set; }

    public MinMaxRangeAttribute(float min, float max)
    {
        Min = min;
        Max = max;
    }
}
