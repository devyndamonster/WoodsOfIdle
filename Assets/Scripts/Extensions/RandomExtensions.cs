using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RandomExtensions
{
    public static float NextFloat(this System.Random random)
    {
        return (float)random.NextDouble();
    }
}
