using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WoodsOfIdle
{
    [CreateAssetMenu(fileName = "ItemData", menuName = "Data/ItemData")]
    public class ItemDataScriptable : ScriptableObject
    {
        public ItemData Value;
    }
}
