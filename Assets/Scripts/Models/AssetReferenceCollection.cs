using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace WoodsOfIdle
{
    [CreateAssetMenu(fileName = "AssetReferenceCollection", menuName = "Data/AssetReferenceCollection")]
    public class AssetReferenceCollection : ScriptableObject
    {
        public List<AssetReference> ItemData;
        public List<AssetReference> FarmingNodePrefabs;
    }
}
