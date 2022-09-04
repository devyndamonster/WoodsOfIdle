using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace WoodsOfIdle
{
    public class AddressableLoaderService : IAddressableLoaderService
    {

        public IEnumerator LoadAssets(AssetReferenceCollection assetCollection, Action<AssetReferenceCollection> onLoaded)
        {
            yield return LoadAssets<ItemData>(assetCollection.ItemDataReferences, (loadedItemData) =>
            {
                assetCollection.LoadedItemData = new List<ItemData>(loadedItemData);
            });

            yield return LoadAssets<GameObject>(assetCollection.FarmingNodePrefabReferences, (loadedFarmingNodePrefabs) =>
            {
                assetCollection.LoadedFarmingNodePrefabs = new List<GameObject>(loadedFarmingNodePrefabs);
            });

            onLoaded(assetCollection);
        }

        public IEnumerator LoadAssets<T>(IEnumerable<AssetReference> references, Action<IEnumerable<T>> onLoaded)
        {
            AsyncOperationHandle<IList<IResourceLocation>> locationsHandle = Addressables.LoadResourceLocationsAsync(references, Addressables.MergeMode.Union, typeof(T));
            yield return locationsHandle;
            
            AsyncOperationHandle<IList<T>> objectsHandle = Addressables.LoadAssetsAsync<T>(locationsHandle.Result, null);
            yield return objectsHandle;

            onLoaded(objectsHandle.Result);
        }
    }
}
