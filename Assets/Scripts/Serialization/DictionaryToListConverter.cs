using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WoodsOfIdle
{


    /// <summary>
    /// Contents based on this github repo (MIT License): https://github.com/hypodwarf/JsonNetConverters
    /// </summary>
    public class DictionaryToListConverter : JsonConverter
    {
        private static (Type kvp, Type list, Type enumerable, Type[] args) GetTypes(Type objectType)
        {
            var args = objectType.GenericTypeArguments;
            var kvpType = typeof(KeyValuePair<,>).MakeGenericType(args);
            var listType = typeof(List<>).MakeGenericType(kvpType);
            var enumerableType = typeof(IEnumerable<>).MakeGenericType(kvpType);

            return (kvpType, listType, enumerableType, args);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var (kvpType, listType, _, args) = GetTypes(value.GetType());

            var keys = ((IDictionary)value).Keys.GetEnumerator();
            var values = ((IDictionary)value).Values.GetEnumerator();
            var cl = listType.GetConstructor(Array.Empty<Type>());
            var ckvp = kvpType.GetConstructor(args);

            var list = (IList)cl!.Invoke(Array.Empty<object>());
            while (keys.MoveNext() && values.MoveNext())
            {
                list.Add(ckvp!.Invoke(new[] { keys.Current, values.Current }));
            }

            serializer.Serialize(writer, list);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var (_, listType, enumerableType, args) = GetTypes(objectType);

            var list = ((IList)(serializer.Deserialize(reader, listType)));

            var ci = objectType.GetConstructor(new[] { enumerableType });
            if (ci == null)
            {
                ci = typeof(Dictionary<,>).MakeGenericType(args).GetConstructor(new[] { enumerableType });
            }

            var dict = (IDictionary)ci!.Invoke(new object[] { list });

            return dict;
        }

        public override bool CanConvert(Type objectType)
        {
            if (!objectType.IsGenericType)
            {
                return false;
            }

            if (objectType.GetGenericTypeDefinition() != typeof(Dictionary<,>))
            {
                return false;
            }

            return true;
        }
    }
}
