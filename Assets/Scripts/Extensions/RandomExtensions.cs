using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class RandomExtensions
{
    public static float NextFloat(this System.Random random)
    {
        return (float)random.NextDouble();
    }

    public static TSource GetRandomFromWeight<TSource>(this IEnumerable<TSource> source, Func<TSource, float> weightSelector)
    {
        float combinedWeight = source.Sum(x => weightSelector(x));
        float randomValue = UnityEngine.Random.Range(0f, combinedWeight);
        float weightSum = 0f;
        
        foreach (var item in source)
        {
            weightSum += weightSelector(item);

            if (weightSum >= randomValue)
            {
                return item;
            }
        }

        return source.Last();
    }
}
