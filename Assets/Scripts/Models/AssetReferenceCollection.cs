using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace WoodsOfIdle
{
    [System.Serializable]
    public class AssetReferenceCollection
    {
        public List<AssetReference> ItemDataReferences;
        public List<AssetReference> FarmingNodeDataReferences;
        public List<AssetReference> FarmingNodePrefabReferences;

        [HideInInspector] public Dictionary<ItemType, ItemData> LoadedItemData;
        [HideInInspector] public Dictionary<FarmingNodeType, FarmingNodeData> LoadedFarmingNodeData;
        [HideInInspector] public Dictionary<FarmingNodeType, GameObject> LoadedFarmingNodePrefabs;
    }
}
