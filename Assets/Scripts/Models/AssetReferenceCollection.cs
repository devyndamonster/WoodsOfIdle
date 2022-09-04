using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace WoodsOfIdle
{
    [CreateAssetMenu(fileName = "AssetReferenceCollection", menuName = "Data/AssetReferenceCollection")]
    public class AssetReferenceCollection : ScriptableObject
    {
        public List<AssetReference> ItemDataReferences;
        public List<AssetReference> FarmingNodePrefabReferences;

        [HideInInspector] public List<ItemData> LoadedItemData;
        [HideInInspector] public List<GameObject> LoadedFarmingNodePrefabs;
    }
}
