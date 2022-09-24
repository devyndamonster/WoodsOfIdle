using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WoodsOfIdle
{
    [CreateAssetMenu(fileName = "AssetReferenceCollection", menuName = "Data/AssetReferenceCollection")]
    public class AssetReferenceCollectionScriptable : ScriptableObject
    {
        public AssetReferenceCollection Value;
    }
}
